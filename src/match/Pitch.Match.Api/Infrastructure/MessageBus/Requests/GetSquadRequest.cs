using System;

namespace Pitch.Match.API.Infrastructure.MessageBus.Requests
{
    public class GetSquadRequest
    {
        public GetSquadRequest(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}
