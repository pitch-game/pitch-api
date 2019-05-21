using System;

namespace Pitch.Card.Api.Infrastructure.Requests
{
    public class CreateCardRequest
    {
        public Guid UserId { get; set; }
        public (int? lower, int? upper)? RatingRange { get; set; }
        public string Position { get; set; }
    }
}
