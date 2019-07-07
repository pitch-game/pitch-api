using Pitch.Match.Api.Models;
using System;
using System.Linq;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class RatingHelper
    {
        public static int CurrentRating(PositionalArea positionalArea, Squad squad)
        {
            var players = squad.Lineup.Where(x => x.Key == positionalArea.ToString()).SelectMany(x => x.Value).ToList();
            var onTheField = players.Where(x => !x.SentOff).ToList(); //Ignore sent off players in rating calcs
            if (onTheField.Count == 0 || players.Count == 0)
                return 0;
            return (int)Math.Round((onTheField.Sum(x => x.Rating * 0.7) + onTheField.Sum(x => x.Fitness * 0.3)) / players.Count);
        }

        public static int CurrentRating(Guid cardId, Squad squad)
        {
            var card = squad.Lineup.SelectMany(x => x.Value).FirstOrDefault(x => x.Id == cardId);
            return (int)Math.Round((card.Rating * 0.7) + (card.Fitness * 0.3));
        }
    }
}
