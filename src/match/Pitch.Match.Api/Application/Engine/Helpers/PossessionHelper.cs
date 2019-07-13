using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class PossessionHelper
    {
        public static Squad InPossession(Models.Match match, out Squad notInPossession, out int homeChance, out int awayChance)
        {
            homeChance = PossessionChance(match.HomeTeam.Squad, match.Events);
            awayChance = PossessionChance(match.AwayTeam.Squad, match.Events);

            //var difference = Math.Abs(homeChance - awayChance);

            //homePercent = (int)Math.Round((1 - ((double)difference / (double)homeChance)) * 100);
            //awayPercent = (int)Math.Round((1 - ((double)difference / (double)awayChance)) * 100);

            var accumulatedWeight = homeChance + awayChance;

            var homePossession = false;

            var rand = new Random();
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= homeChance)
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

        private static int PossessionChance(Squad squad, IEnumerable<IEvent> events)
        {
            return (int)Math.Round((RatingHelper.CurrentRating(PositionalArea.GK, squad, events) * 0.1) +
                (RatingHelper.CurrentRating(PositionalArea.DEF, squad, events) * 0.5) +
                (RatingHelper.CurrentRating(PositionalArea.MID, squad, events) * 1) +
                (RatingHelper.CurrentRating(PositionalArea.ATT, squad, events) * 0.5));
        }
    }
}
