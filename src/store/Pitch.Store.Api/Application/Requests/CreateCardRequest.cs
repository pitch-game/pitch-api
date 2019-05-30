using System;

namespace Pitch.Store.Api.Application.Requests
{
    public class CreateCardRequest
    {
        public CreateCardRequest(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; set; }
        public (int? lower, int? upper)? RatingRange { get; set; }
        public string Position { get; set; }
    }
}
