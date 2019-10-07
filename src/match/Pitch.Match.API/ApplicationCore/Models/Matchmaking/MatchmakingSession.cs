using System;

namespace Pitch.Match.API.ApplicationCore.Models.Matchmaking
{
    public class MatchmakingSession
    {
        private const int SessionLengthInMinutes = 10; //TODO move to constants class

        public Guid Id { get; set; }
        public Guid HostPlayerId { get; set; }
        public Guid? JoinedPlayerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public bool Expired => DateTime.Now > CreatedOn.AddMinutes(SessionLengthInMinutes);
        public bool Open => JoinedPlayerId == null;
    }
}
