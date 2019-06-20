using System;

namespace Pitch.Match.Api.Application.MessageBus.Events
{
    public class MatchCompletedEvent
    {
        public Guid MatchId { get; set; }
        public Guid UserId { get; set; }
        public bool Victorious { get; set; }
        public int GoalDifference { get; set; }
        public bool CleanSheet { get; set; }
    }
}
