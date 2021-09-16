using System;

namespace Pitch.Match.Engine.Models
{
    public class TeamDetails
    {
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public bool HasClaimedRewards { get; set; }
        public virtual int UsedSubs { get; set; }
    }
}