using EasyNetQ;
using Microsoft.AspNetCore.SignalR;
using Pitch.Match.Api.Application.MessageBus.Requests;
using Pitch.Match.Api.Application.MessageBus.Responses;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Models.Matchmaking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Services
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
        private readonly IBus _bus;
        private readonly IMatchSessionService _matchSessionService;

        public MatchmakingService(IMatchRepository matchRepository, IBus bus, IMatchSessionService matchSessionService)
        {
            _matchRepository = matchRepository;
            _bus = bus;
            _matchSessionService = matchSessionService;
        }

        public async Task<MatchmakingSession> Matchmake(Guid userId)
        {

            //var squad = await _bus.RequestAsync<GetSquadRequest, GetSquadResponse>(new GetSquadRequest(userId));
            var matchInProgress = await _matchRepository.GetInProgressAsync(userId);

            if (matchInProgress.HasValue) //|| squad == null)
                throw new Exception("User cannot matchmake");

            var existing = _matchSessionService.Sessions.FirstOrDefault(x => x.Open && !x.Expired && x.HostPlayerId != userId);
            if (existing != null)
            {
                return JoinSession(existing.Id, userId);
            }
            else
            {
                return CreateSession(userId);
            }
        }

        public MatchmakingSession GetSession(Guid id)
        {
            return _matchSessionService.Sessions.FirstOrDefault(x => x.Id == id);
        }

        public MatchmakingSession JoinSession(Guid sessionId, Guid playerId)
        {
            var session = _matchSessionService.Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (playerId == session.HostPlayerId) {
                throw new HubException("Host attempted to join own session");
            }
            session.JoinedPlayerId = playerId;
            session.CompletedOn = DateTime.Now;
            return session;
        }

        public MatchmakingSession CreateSession(Guid userId)
        {
            var session = new MatchmakingSession()
            {
                Id = Guid.NewGuid(),
                HostPlayerId = userId,
                CreatedOn = DateTime.Now
            };
            _matchSessionService.Sessions.Add(session);
            return session;
        }

        public void Cancel(Guid sessionId)
        {
            var session = _matchSessionService.Sessions.FirstOrDefault(x => x.Id == sessionId);
            _matchSessionService.Sessions.Remove(session);
        }
    }
}
