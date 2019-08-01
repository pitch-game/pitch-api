using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.MessageBus.Events
{
    public class MatchCompletedEvent
    {
        public MatchCompletedEvent(Guid matchId, Guid userId, bool victorious, IDictionary<Guid, int> scorers)
        {
            MatchId = matchId;
            UserId = userId;
            Victorious = victorious;
            Scorers = scorers;
        }

        public Guid MatchId { get; set; }
        public Guid UserId { get; set; }
        public bool Victorious { get; set; }
        public IDictionary<Guid, int> Scorers { get; set; }
    }
}
