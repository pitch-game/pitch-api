using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    //TODO Refactor as service and use DI
    public static class RatingHelper
    {
        public static int CurrentRating(PositionalArea positionalArea, Squad squad, IEnumerable<IEvent> events)
        {
            var players = squad.Lineup.Where(x => x.Value != null && x.Key == positionalArea.ToString()).SelectMany(x => x.Value).ToList();

            var sentOffCardIds = events.Where(x => x.GetType() == typeof(RedCard)).Select(x => x.CardId);
            var onTheField = players.Where(x => x != null && !sentOffCardIds.Contains(x.Id)).ToList();

            if (onTheField.Count == 0 || players.Count == 0)
                return 0;
            return (int)Math.Round((onTheField.Sum(x => x.Rating * 0.7) + onTheField.Sum(x => x.Fitness * 0.3)) / players.Count);
        }

        public static int CurrentRating(Guid cardId, Squad squad)
        {
            var card = squad.Lineup.SelectMany(x => x.Value).FirstOrDefault(x => x.Id == cardId);
            return (int)Math.Round((card.Rating * 0.33) + (card.Fitness * 0.33) + (card.Chemistry * 0.33));
        }
    }
}
