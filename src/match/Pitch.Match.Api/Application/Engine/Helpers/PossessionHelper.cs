using Pitch.Match.Api.Models;
using System;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class PossessionHelper
    {
        public static Squad InPossession(Models.Match match, out Squad notInPossession, out int homePercent, out int awayPercent)
        {
            var homeChance = PossessionChance(match.HomeTeam.Squad);
            var awayChance = PossessionChance(match.AwayTeam.Squad);

            var difference = Math.Abs(homeChance - awayChance);

            //TODO this is a bit dramatic

            homePercent = (int)Math.Round(100 - (((double)difference / (double)homeChance) * 100));
            awayPercent = (int)Math.Round(100 - (((double)difference / (double)awayChance) * 100));

            var accumulatedWeight = homePercent + awayPercent;

            var homePossession = false;

            var rand = new Random();
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= homePercent)
            {
                homePossession = true;
            }

            if (homePossession)
            {
                notInPossession = match.AwayTeam.Squad;
                return match.HomeTeam.Squad;
            }
            else
            {
                notInPossession = match.HomeTeam.Squad;
                return match.AwayTeam.Squad;
            }
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
