using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.User.Api.Application.Events
{
    public class MatchCompletedEvent
    {
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
