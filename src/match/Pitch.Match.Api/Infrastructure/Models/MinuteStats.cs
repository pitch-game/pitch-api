using System;

namespace Pitch.Match.Api.Infrastructure.Models
{
    public class MinuteStats
    {
        public Guid SquadIdInPossession { get; set; }
        public int HomePossessionChance { get; set; }
        public int AwayPossessionChance { get; set; }
    }
}
