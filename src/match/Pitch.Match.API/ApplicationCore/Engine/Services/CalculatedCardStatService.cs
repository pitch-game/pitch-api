using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    public interface ICalculatedCardStatService
    {
        int Fitness(Models.Match.Match match, Guid cardId);
    }

    public class CalculatedCardStatService : ICalculatedCardStatService
    {
        private const int FitnessUpperBound = 100;

        public int Fitness(Models.Match.Match match, Guid cardId)
        {
            return (int)Math.Round(FitnessUpperBound - EffectiveModifiers(match, cardId, ModifierType.Fitness).Sum(x => x.DrainValue));
        }

        private static IEnumerable<Modifier> EffectiveModifiers(Models.Match.Match match, Guid cardId, ModifierType type)
        {
            return match.Minutes.SelectMany(x => x.Modifiers).Where(x => x.CardId == cardId && x.Type == type);
        }
    }
}
