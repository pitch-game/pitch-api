using EasyNetQ;
using Pitch.Match.Api.Application.Engine;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Application.MessageBus.Events;
using Pitch.Match.Api.Application.MessageBus.Requests;
using Pitch.Match.Api.Application.MessageBus.Responses;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Services
{
    public interface IMatchService
    {
        Task KickOff(Guid sessionId);
        Task<Models.Match> GetAsync(Guid id);
        Task ClaimAsync(Guid userId);
        Task<IEnumerable<Models.MatchListResult>> GetAllAsync(int skip, int? take, Guid userId);
        Task<Models.MatchStatusResult> GetMatchStatus(Guid userId);
    }

    public class MatchService : IMatchService
    {
        private readonly IMatchmakingService _matchmakingService;
        private readonly IMatchEngine _matchEngine;
        private readonly IMatchRepository _matchRepository;
        private readonly IBus _bus;

        public MatchService(IMatchmakingService matchmakingService, IMatchEngine matchEngine, IMatchRepository matchRepository, IBus bus)
        {
            _matchmakingService = matchmakingService;
            _matchEngine = matchEngine;
            _matchRepository = matchRepository;
            _bus = bus;
        }

        public async Task ClaimAsync(Guid userId)
        {
            var unclaimed = await _matchRepository.GetUnclaimedAsync(userId);
            var scorers = new Dictionary<Guid, int>();
            foreach (var match in unclaimed)
            {
                var victorious = false;
                if (match.AwayTeam.UserId == userId)
                {
                    match.AwayTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.AwayResult.Score > matchResult.HomeResult.Score;
                    scorers = match.Events.Where(x => x.SquadId == match.AwayTeam.Squad.Id && x.GetType() == typeof(Goal)).GroupBy(x => x.CardId).ToDictionary(x => x.Key, x => x.Count());
                }
                else if (match.HomeTeam.UserId == userId)
                {
                    match.HomeTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.HomeResult.Score > matchResult.AwayResult.Score;
                    scorers = match.Events.Where(x => x.SquadId == match.HomeTeam.Squad.Id && x.GetType() == typeof(Goal)).GroupBy(x => x.CardId).ToDictionary(x => x.Key, x => x.Count());
                }

                await _bus.PublishAsync(new MatchCompletedEvent(match.Id, userId, victorious, scorers));
                await _matchRepository.UpdateAsync(match);
            }
        }

        public async Task<Models.Match> GetAsync(Guid id)
        {
            return await _matchRepository.GetAsync(id);
        }

        public async Task KickOff(Guid sessionId)
        {
            var session = _matchmakingService.GetSession(sessionId);

            var match = new Models.Match
            {
                Id = sessionId
            };

            match.HomeTeam = new TeamDetails
            {
                UserId = session.HostPlayerId
            };
            match.HomeTeam.Squad = BuildSquad(await _bus.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(match.HomeTeam.UserId)));

            match.AwayTeam = new TeamDetails
            {
                UserId = session.JoinedPlayerId.Value
            };
            match.AwayTeam.Squad = BuildSquad(await _bus.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(match.AwayTeam.UserId)));

            match.KickOff = DateTime.Now;

            var simulatedMatch = _matchEngine.SimulateReentrant(match);

            await _matchRepository.CreateAsync(simulatedMatch);
        }

        private Squad BuildSquad(GetSquadResponse squadResp)
        {
            var gk = squadResp.Cards.Where(x => x.Position == "GK").ToList();
            var def = squadResp.Cards.Where(x => (new string[] { "LB", "LCB", "RCB", "RB" }).Contains(x.Position)).ToList();
            var mid = squadResp.Cards.Where(x => (new string[] { "LM", "LCM", "RCM", "RM" }).Contains(x.Position)).ToList();
            var att = squadResp.Cards.Where(x => (new string[] { "LST", "RST" }).Contains(x.Position)).ToList();

            return new Squad()
            {
                Id = squadResp.Id,
                Name = squadResp.Name,
                Lineup = new Dictionary<string, IEnumerable<Card>>()
                {
                    { "GK", gk },
                    { "DEF", def },
                    { "MID" , mid },
                    { "ATT", att }
                }
            };
        }

        public async Task<IEnumerable<Models.MatchListResult>> GetAllAsync(int skip, int? take, Guid userId)
        {
            var matches = await _matchRepository.GetAllAsync(skip, take ?? 25, userId);
            matches = matches.Where(x => x.IsOver);
            return matches.Select(x =>
            {
                var matchResult = new MatchResult(x);
                var isHomeTeam = x.HomeTeam.UserId == userId;
                var claimed = isHomeTeam  ? x.HomeTeam.HasClaimedRewards : x.AwayTeam.HasClaimedRewards;
                var result = matchResult.HomeResult.Score == matchResult.AwayResult.Score ? "D" : isHomeTeam ? matchResult.HomeResult.Score > matchResult.AwayResult.Score ? "W" : "L" : matchResult.AwayResult.Score > matchResult.HomeResult.Score ? "W" : "L";
                return new Models.MatchListResult
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
    }
}
