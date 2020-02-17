using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    public interface IRatingService
    {
        int CurrentRating(PositionalArea positionalArea, Models.Match match, Squad squad);
        int CurrentRating(Guid cardId, Models.Match match, Squad squad);
    }

    public class RatingService : IRatingService
    {
        private readonly ICalculatedCardStatService _calculatedCardStatService;

        public RatingService(ICalculatedCardStatService calculatedCardStatService)
        {
            _calculatedCardStatService = calculatedCardStatService;
        }

        public int CurrentRating(PositionalArea positionalArea, Models.Match match, Squad squad)
        {
            var players = squad.Lineup.Where(x => x.Value != null && x.Key == positionalArea.ToString()).SelectMany(x => x.Value).ToList();

            var sentOffCardIds = match.Minutes.SelectMany(x => x.Events).Where(x => x is RedCard).Select(x => x.CardId);
            var onTheField = players.Where(x => x != null && !sentOffCardIds.Contains(x.Id)).ToList();

            if (onTheField.Count == 0 || players.Count == 0)
                return 0;
            return (int)Math.Round((onTheField.Sum(x => x.Rating * 0.7) + onTheField.Sum(x => _calculatedCardStatService.Fitness(match, x.Id) * 0.3)) / players.Count);
        }

        public int CurrentRating(Guid cardId, Models.Match match, Squad squad)
        {
            var card = squad.Lineup.SelectMany(x => x.Value).FirstOrDefault(x => x.Id == cardId);
            return (int)Math.Round(card.Rating * 0.33 + _calculatedCardStatService.Fitness(match, card.Id) * 0.33 + card.Chemistry * 0.33);
        }
    }
}
