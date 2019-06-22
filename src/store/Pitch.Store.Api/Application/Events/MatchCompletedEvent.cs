using System;
using System.Collections.Generic;

namespace Pitch.Store.Api.Application.Events
{
    public class MatchCompletedEvent
    {
        public MatchCompletedEvent()
        {
                
        }
        public MatchCompletedEvent(Guid matchId, Guid userId, bool victorious)
        {
            MatchId = matchId;
            UserId = userId;
            Victorious = victorious;
        }

        public Guid MatchId { get; set; }
        public Guid UserId { get; set; }
        public bool Victorious { get; set; }
    }
}
