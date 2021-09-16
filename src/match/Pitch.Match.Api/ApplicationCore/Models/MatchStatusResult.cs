using System;

namespace Pitch.Match.Api.ApplicationCore.Models
{
    public class MatchStatusResult
    {
        public bool HasUnclaimedRewards { get; set; }
        public Guid? InProgressMatchId { get; set; }
    }
}
