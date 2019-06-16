using Pitch.Match.Api.Models;
using System;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class PossessionHelper
    {
        public static Squad InPossession(Models.Match match, out Squad notInPossession)
        {
            var team1Chance = PossessionChance(match.HomeTeam);
            var team2Chance = PossessionChance(match.AwayTeam);

            var difference = Math.Abs(team1Chance - team2Chance);

            var team1Percent = (int)Math.Round(100 - (((double)difference / (double)team1Chance) * 100));
            var team2Percent = (int)Math.Round(100 - (((double)difference / (double)team2Chance) * 100));

            var accumulatedWeight = team1Percent + team2Percent;

            var team1InPossession = false;

            var rand = new Random();
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= team1Percent)
            {
                team1InPossession = true;
            }

            if (team1InPossession)
            {
                notInPossession = match.AwayTeam;
                return match.HomeTeam;
            }
            else
            {
                notInPossession = match.HomeTeam;
                return match.AwayTeam;
            }
            //record possession stats
        }

        private static int PossessionChance(Squad squad)
        {
            return (int)Math.Round((RatingHelper.CurrentRating(PositionalArea.GK, squad) * 0.1) +
                (RatingHelper.CurrentRating(PositionalArea.DEF, squad) * 0.5) +
                (RatingHelper.CurrentRating(PositionalArea.MID, squad) * 1) +
                (RatingHelper.CurrentRating(PositionalArea.ATT, squad) * 0.5));
        }
    }
}
