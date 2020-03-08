using System;

namespace Pitch.Match.API.ApplicationCore.Models.Match
{
    public class TeamDetails
    {
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public bool HasClaimedRewards { get; set; }
        public virtual int UsedSubs { get; set; }
    }
}