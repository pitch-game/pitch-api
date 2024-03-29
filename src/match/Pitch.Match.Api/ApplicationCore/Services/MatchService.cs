﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using EasyNetQ;
using Pitch.Match.Api.ApplicationCore.Models;
using Pitch.Match.Api.ApplicationCore.Models.MatchResult;
using Pitch.Match.Api.Infrastructure.Mapping;
using Pitch.Match.Api.Infrastructure.MessageBus.Events;
using Pitch.Match.Api.Infrastructure.MessageBus.Requests;
using Pitch.Match.Api.Infrastructure.MessageBus.Responses;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Engine;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Services;
using Squad = Pitch.Match.Engine.Models.Squad;

namespace Pitch.Match.Api.ApplicationCore.Services
{
    public interface IMatchService
    {
        Task KickOff(Guid sessionId);
        Task<Engine.Models.Match> GetAsAtElapsedAsync(Guid id);
        Task ClaimAsync(Guid userId);
        Task<IEnumerable<MatchListResult>> GetAllAsync(int skip, int? take, Guid userId);
        Task<MatchStatusResult> GetMatchStatus(Guid userId);
        Task<ApplicationCore.Models.Lineup> GetLineupAsync(Guid matchId, Guid userId);
        Task Substitution(Guid off, Guid on, Guid matchId, Guid userId);
    }

    public class MatchService : IMatchService
    {
        private readonly IMatchmakingService _matchmakingService;
        private readonly IMatchEngine _matchEngine;
        private readonly IMatchRepository _matchRepository;
        private readonly ICalculatedCardStatService _calculatedCardStatService;
        private readonly IBus _bus;
        private readonly IMatchDtoMapper _matchDtoMapper;
        private readonly IMatchMapper _matchMapper;

        public const int SubCount = 3;

        public MatchService(IMatchmakingService matchmakingService, IMatchEngine matchEngine, IMatchRepository matchRepository, IBus bus, ICalculatedCardStatService calculatedCardStatService, IMatchDtoMapper matchDtoMapper, IMatchMapper matchMapper)
        {
            _matchmakingService = matchmakingService;
            _matchEngine = matchEngine;
            _matchRepository = matchRepository;
            _bus = bus;
            _calculatedCardStatService = calculatedCardStatService;
            _matchDtoMapper = matchDtoMapper;
            _matchMapper = matchMapper;
        }

        //TODO remove usage of match result model
        public async Task ClaimAsync(Guid userId)
        {
            var unclaimed = await _matchRepository.GetUnclaimedAsync(userId);
            var scorers = new Dictionary<Guid, int>();
            foreach (var matchDto in unclaimed)
            {
                var match = _matchDtoMapper.Map(matchDto);
                var victorious = false;
                if (match.AwayTeam.UserId == userId)
                {
                    match.AwayTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.AwayResult.Score > matchResult.HomeResult.Score;
                    scorers = match.Minutes.SelectMany(x => x.Events).Where(x => x.SquadId == match.AwayTeam.Squad.Id && x.Type == EventType.Goal).GroupBy(x => x.CardId).ToDictionary(x => x.Key, x => x.Count());
                }
                else if (match.HomeTeam.UserId == userId)
                {
                    match.HomeTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.HomeResult.Score > matchResult.AwayResult.Score;
                    scorers = match.Minutes.SelectMany(x => x.Events).Where(x => x.SquadId == match.HomeTeam.Squad.Id && x.Type == EventType.Goal).GroupBy(x => x.CardId).ToDictionary(x => x.Key, x => x.Count());
                }

                await _bus.PubSub.PublishAsync(new MatchCompletedEvent(match.Id, userId, victorious, scorers));

                var matchUpdate = _matchMapper.Map(match);
                await _matchRepository.UpdateAsync(matchUpdate);
            }
        }

        public async Task<Engine.Models.Match> GetAsAtElapsedAsync(Guid id)
        {
            var matchDto = await _matchRepository.GetAsync(id);
            var match = _matchDtoMapper.Map(matchDto);

            match.AsAtElapsed();
            _calculatedCardStatService.Set(match);
            return match;
        }

        public async Task KickOff(Guid sessionId)
        {
            var session = _matchmakingService.GetSession(sessionId);

            var match = new Engine.Models.Match
            {
                Id = sessionId, HomeTeam = new TeamDetails { UserId = session.HostPlayerId }
            };

            match.HomeTeam.Squad = BuildSquad(await _bus.Rpc.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(match.HomeTeam.UserId)));

            match.AwayTeam = new TeamDetails
            {
                UserId = session.JoinedPlayerId.Value
            };
            match.AwayTeam.Squad = BuildSquad(await _bus.Rpc.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(match.AwayTeam.UserId)));

            match.KickOff = DateTime.Now;

            var simulatedMatch = _matchEngine.Simulate(match);

            var matchDto = _matchMapper.Map(simulatedMatch);

            await _matchRepository.CreateAsync(matchDto);
        }

        private Squad BuildSquad(GetSquadResponse squadResp)
        {
            var gk = squadResp.Lineup.Where(x => x.Key == "GK").Select(x => x.Value).ToList();
            var def = squadResp.Lineup.Where(x => (new [] { "LB", "LCB", "RCB", "RB" }).Contains(x.Key)).Select(x => x.Value).ToList();
            var mid = squadResp.Lineup.Where(x => (new [] { "LM", "LCM", "RCM", "RM" }).Contains(x.Key)).Select(x => x.Value).ToList();
            var att = squadResp.Lineup.Where(x => (new [] { "LST", "RST" }).Contains(x.Key)).Select(x => x.Value).ToList();

            return new Squad()
            {
                Id = squadResp.Id,
                Name = squadResp.Name,
                Lineup = new Dictionary<string, IEnumerable<Card>>()
                {
                    { "GK", gk },
                    { "DEF", def },
                    { "MID", mid },
                    { "ATT", att }
                },
                Subs = squadResp.Subs
            };
        }

        //TODO remove usage of match result model
        public async Task<IEnumerable<MatchListResult>> GetAllAsync(int skip, int? take, Guid userId)
        {
            var matches = await _matchRepository.GetAllAsync(skip, take ?? 25, userId);
            //matches = matches.Where(x => x.HasFinished);
            return matches.Select(x =>
            {
                var match = _matchDtoMapper.Map(x);
                if (match == null || !match.HasFinished)
                {
                    //continue;
                }
                var matchResult = new MatchResult(match);
                var isHomeTeam = x.HomeTeam.UserId == userId;
                var claimed = isHomeTeam ? x.HomeTeam.HasClaimedRewards : x.AwayTeam.HasClaimedRewards;
                var result = matchResult.HomeResult.Score == matchResult.AwayResult.Score ? "D" : isHomeTeam ? matchResult.HomeResult.Score > matchResult.AwayResult.Score ? "W" : "L" : matchResult.AwayResult.Score > matchResult.HomeResult.Score ? "W" : "L";
                return new MatchListResult
                {
                    Id = x.Id,
                    HomeTeam = x.HomeTeam.Squad.Name,
                    AwayTeam = x.AwayTeam.Squad.Name,
                    HomeScore = matchResult.HomeResult.Score,
                    AwayScore = matchResult.AwayResult.Score,
                    KickOff = x.KickOff,
                    Result = result,
                    Claimed = claimed
                };
            });
        }

        public async Task<MatchStatusResult> GetMatchStatus(Guid userId)
        {
            var hasUnclaimedRewards = await _matchRepository.HasUnclaimedAsync(userId);
            var inProgressMatchId = await _matchRepository.GetInProgressAsync(userId);
            return new MatchStatusResult
            {
                HasUnclaimedRewards = hasUnclaimedRewards,
                InProgressMatchId = inProgressMatchId
            };
        }

        public async Task<Models.Lineup> GetLineupAsync(Guid matchId, Guid userId)
        {
            var match = await GetAsAtElapsedAsync(matchId);
            var team = match.GetTeam(userId);
            var sendingOffs = match.Minutes.SelectMany(x => x.Events).Where(x => x.Type == EventType.RedCard).Select(x => x.CardId);
            var lineup = team.Squad.Lineup.Values.SelectMany(x => x).Where(x => !sendingOffs.Contains(x.Id)).ToList();
            return new Models.Lineup { Active = lineup, Subs = team.Squad.Subs };
        }

        public async Task Substitution(Guid off, Guid on, Guid matchId, Guid userId)
        {
            //TODO validate
            var matchDto = await _matchRepository.GetAsync(matchId);

            var match = _matchDtoMapper.Map(matchDto);

            var team = match.GetTeam(userId);

            if (team.UsedSubs >= SubCount) throw new Exception("No subs remaining");

            match.Substitute(off, on, userId);
            var newMatch = _matchEngine.Simulate(match);

            newMatch.GetTeam(userId).UsedSubs++;

            var updatedMatchDto = _matchMapper.Map(newMatch);

            await _matchRepository.UpdateAsync(updatedMatchDto);
        }
    }
}
