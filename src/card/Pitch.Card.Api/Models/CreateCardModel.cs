using System;

namespace Pitch.Card.Api.Models
{
    public class CreateCardModel
    {
        public string UserId { get; set; }
        public (int? lower, int? upper)? RatingRange { get; set; }
        public string Position { get; set; }
    }
}
