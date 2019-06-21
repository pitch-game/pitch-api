﻿using System;

namespace Pitch.Match.Api.Application.MessageBus.Events
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
        public int Scorers { get; set; }
    }
}
