using Microsoft.AspNetCore.SignalR;
using Pitch.Match.Api.Models.Matchmaking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Services
{
    public interface IMatchmakingService
    {
        MatchmakingSession Matchmake(Guid userId);
        void Cancel(Guid userId);
        MatchmakingSession GetSession(Guid id);
    }

    public class MatchmakingService : IMatchmakingService
    {
        private static IList<MatchmakingSession> Sessions; //TODO ordered list? for ordering or stack?

        public MatchmakingService()
        {
            Sessions = new List<MatchmakingSession>();
        }

        public MatchmakingSession Matchmake(Guid userId)
        {
            var existing = Sessions.FirstOrDefault(x => x.Open && !x.Expired);
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
            return Sessions.FirstOrDefault(x => x.Id == id);
        }

        public MatchmakingSession JoinSession(Guid sessionId, Guid playerId)
        {
            var session = Sessions.FirstOrDefault(x => x.Id == sessionId);
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
            Sessions.Add(session);
            return session;
        }

        public void Cancel(Guid sessionId)
        {
            var session = Sessions.FirstOrDefault(x => x.Id == sessionId);
            Sessions.Remove(session);
        }
    }
}
