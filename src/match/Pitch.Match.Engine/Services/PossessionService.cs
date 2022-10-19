using System;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Engine.Services
{
    public interface IPossessionService
    {
        Squad InPossession(Models.Match match, out Squad notInPossession, out int homeChance, out int awayChance);
    }

    public class PossessionService : IPossessionService
    {
        private readonly IRatingService _ratingService;

        public PossessionService(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public Squad InPossession(Models.Match match, out Squad notInPossession, out int homeChance, out int awayChance)
        {
            homeChance = PossessionChance(match.HomeTeam.Squad, match);
            awayChance = PossessionChance(match.AwayTeam.Squad, match);

            var homePossession = ChanceHelper.CumulativeTrueOrFalse(homeChance, awayChance);
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

        private int PossessionChance(Squad squad, Models.Match match)
        {
            return (int)Math.Round((_ratingService.CurrentRating(PositionalArea.GK, match, squad) * Constants.PossessionChanceModifierGoalkeeper) +
                (_ratingService.CurrentRating(PositionalArea.DEF, match, squad) * Constants.PossessionChanceModifierDefence) +
                (_ratingService.CurrentRating(PositionalArea.MID, match, squad) * Constants.PossessionChanceModifierMidfield) +
                (_ratingService.CurrentRating(PositionalArea.ATT, match, squad) * Constants.PossessionChanceModifierAttack));
        }
    }
}
