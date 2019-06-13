using System;

namespace Pitch.User.Api.Application.Events
{
    public class UserCreatedEvent
    {
        public UserCreatedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
