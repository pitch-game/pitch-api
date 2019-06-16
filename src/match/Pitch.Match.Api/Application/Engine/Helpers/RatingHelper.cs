using Pitch.Match.Api.Models;
using System;
using System.Linq;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class RatingHelper
    {
        public static int CurrentRating(PositionalArea positionalArea, Squad squad)
        {
            var players = squad.Lineup.Where(x => x.Key == positionalArea).SelectMany(x => x.Value).ToList();
            return (int)Math.Round((players.Sum(x => x.Rating * 0.7) + players.Sum(x => x.Fitness * 0.3)) / players.Count);
        }
    }
}
