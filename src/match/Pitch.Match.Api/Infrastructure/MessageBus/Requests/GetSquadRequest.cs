﻿using System;

namespace Pitch.Match.Api.Infrastructure.MessageBus.Requests
{
    public class GetSquadRequest
    {
        public GetSquadRequest(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
