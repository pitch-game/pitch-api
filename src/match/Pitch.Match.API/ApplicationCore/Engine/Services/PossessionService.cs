using System;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    public interface IPossessionService
    {
        Squad InPossession(Models.Match.Match match, out Squad notInPossession, out int homeChance, out int awayChance);
        int PossessionChance(Squad squad, Models.Match.Match match);
    }

    public class PossessionService : IPossessionService
    {
        private readonly IRatingService _ratingService;

        public PossessionService(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public Squad InPossession(Models.Match.Match match, out Squad notInPossession, out int homeChance, out int awayChance)
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

        public int PossessionChance(Squad squad, Models.Match.Match match)
        {
            return (int)Math.Round((_ratingService.CurrentRating(PositionalArea.GK, match, squad) * 0.1) +
                (_ratingService.CurrentRating(PositionalArea.DEF, match, squad) * 0.5) +
                (_ratingService.CurrentRating(PositionalArea.MID, match, squad) * 1) +
                (_ratingService.CurrentRating(PositionalArea.ATT, match, squad) * 0.5));
        }
    }
}
