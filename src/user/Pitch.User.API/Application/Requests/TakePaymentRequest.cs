using System;

namespace Pitch.User.API.Application.Requests
{
    public class TakePaymentRequest
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }
}
