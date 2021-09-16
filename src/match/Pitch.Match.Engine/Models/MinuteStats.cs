using System;

namespace Pitch.Match.Engine.Models
{
    public class MinuteStats
    {
        public MinuteStats(Guid squadIdInPossession, int homePossChance, int awayPossChance)
        {
            SquadIdInPossession = squadIdInPossession;
            HomePossessionChance = homePossChance;
            AwayPossessionChance = awayPossChance;
        }

        public Guid SquadIdInPossession { get; set; }
        public int HomePossessionChance { get; set; }
        public int AwayPossessionChance { get; set; }
    }
}