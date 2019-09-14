using System;

namespace Pitch.Card.API.Application.Requests
{
    public class CreateCardRequest
    {
        public string UserId { get; set; }
        public (int? lower, int? upper)? RatingRange { get; set; }
        public string Position { get; set; }
    }
}
