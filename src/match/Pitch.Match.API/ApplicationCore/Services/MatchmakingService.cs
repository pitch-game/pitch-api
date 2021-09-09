using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pitch.Match.API.ApplicationCore.Models.Matchmaking;
using Pitch.Match.API.Infrastructure.Repositories;

namespace Pitch.Match.API.ApplicationCore.Services
{
    public interface IMatchmakingService
    {
        Task<MatchmakingSession> Matchmake(Guid userId);
        void Cancel(Guid userId);
        MatchmakingSession GetSession(Guid id);
    }

    public class MatchmakingService : IMatchmakingService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchSessionService _matchSessionService;

        public MatchmakingService(IMatchRepository matchRepository, IMatchSessionService matchSessionService)
        {
            _matchRepository = matchRepository;
            _matchSessionService = matchSessionService;
        }

        public async Task<MatchmakingSession> Matchmake(Guid userId)
        {
            //var squad = await _bus.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(userId));
            var matchInProgress = await _matchRepository.GetInProgressAsync(userId);

            if (matchInProgress.HasValue) //|| squad == null)
                throw new Exception("User cannot matchmake");

            var existing =
                _matchSessionService.Sessions.FirstOrDefault(x => x.Open && !x.Expired && x.HostPlayerId != userId);
            if (existing != null)
                return JoinSession(existing.Id, userId);
            return CreateSession(userId);
        }

        public MatchmakingSession GetSession(Guid id)
        {
            return _matchSessionService.Sessions.FirstOrDefault(x => x.Id == id);
        }

        public void Cancel(Guid sessionId)
        {
            var session = _matchSessionService.Sessions.FirstOrDefault(x => x.Id == sessionId);
            _matchSessionService.Sessions.Remove(session);
        }

        private MatchmakingSession JoinSession(Guid sessionId, Guid playerId)
        {
            var session = _matchSessionService.Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (playerId == session.HostPlayerId) throw new HubException("Host attempted to join own session");
            session.JoinedPlayerId = playerId;
            session.CompletedOn = DateTime.Now;
            return session;
        }

        private MatchmakingSession CreateSession(Guid userId)
        {
            var session = new MatchmakingSession
            {
                Id = Guid.NewGuid(),
                HostPlayerId = userId,
                CreatedOn = DateTime.Now
            };
            _matchSessionService.Sessions.Add(session);
            return session;
        }
    }
}