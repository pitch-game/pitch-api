using EasyNetQ;
using Pitch.Match.Api.Application.Engine;
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
        Task<bool> HasUnclaimed(Guid userId);
        Task ClaimAsync(Guid userId);
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
            foreach (var match in unclaimed)
            {
                var victorious = false;
                if (match.AwayTeam.UserId == userId)
                {
                    match.AwayTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.AwayResult.Score > matchResult.HomeResult.Score;
                }
                else if (match.HomeTeam.UserId == userId)
                {
                    match.HomeTeam.HasClaimedRewards = true;
                    var matchResult = new MatchResult(match);
                    victorious = matchResult.HomeResult.Score > matchResult.AwayResult.Score;
                }

                await _bus.PublishAsync(new MatchCompletedEvent(match.Id, userId, victorious));
                await _matchRepository.UpdateAsync(match);
            }
        }

        public async Task<Models.Match> GetAsync(Guid id)
        {
            return await _matchRepository.GetAsync(id);
        }

        public async Task<bool> HasUnclaimed(Guid userId)
        {
            return await _matchRepository.HasUnclaimedAsync(userId);
        }

        public async Task<bool> HasMatchInProgress(Guid userId)
        {
            return await _matchRepository.GetInProgressAsync(userId);
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
                Id = Guid.NewGuid(), //TODO use real squad id
                Lineup = new Dictionary<string, IEnumerable<Card>>()
                {
                    { "GK", gk },
                    { "DEF", def },
                    { "MID" , mid },
                    { "ATT", att }
                }
            };
        }
    }
}
