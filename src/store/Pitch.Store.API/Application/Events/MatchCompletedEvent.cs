﻿using System;

namespace Pitch.Store.API.Application.Events
{
    public class MatchCompletedEvent
    {
        public Guid UserId { get; set; }
        public bool Victorious { get; set; }
    }
}
