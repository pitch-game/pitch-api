using System;

namespace Pitch.Store.API.Application.Requests
{
    public class TakePaymentRequest
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }
}
