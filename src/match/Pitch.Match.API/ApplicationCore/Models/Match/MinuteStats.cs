using System;

namespace Pitch.Match.API.ApplicationCore.Models.Match
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