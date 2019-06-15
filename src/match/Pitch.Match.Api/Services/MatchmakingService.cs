using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Services
{
    public class MatchmakingSession
    {
        private const int SESSION_LENGTH_IN_MINUTES = 10; //TODO move to constants class

        public Guid Id { get; set; }
        public Guid HostPlayerId { get; set; }
        public Guid? JoinedPlayerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public bool Expired => DateTime.Now > CreatedOn.AddMinutes(SESSION_LENGTH_IN_MINUTES);
        public bool Open => JoinedPlayerId == null;
    }

    public interface IMatchmakingService
    {
        MatchmakingSession Matchmake(Guid userId);
        void Cancel(Guid userId);
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

        public MatchmakingSession JoinSession(Guid sessionId, Guid playerId)
        {
            var session = Sessions.FirstOrDefault(x => x.Id == sessionId);
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
