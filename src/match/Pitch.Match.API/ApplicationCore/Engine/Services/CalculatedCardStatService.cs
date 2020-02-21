using System;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    public interface ICalculatedCardStatService
    {
        int Fitness(Models.Match match, Guid cardId);
    }

    public class CalculatedCardStatService : ICalculatedCardStatService
    {
        public int Fitness(Models.Match match, Guid cardId)
        {
            var effectiveModifiers = match.Minutes.SelectMany(x => x.Modifiers).Where(x => x.CardId == cardId && x.Type == ModifierType.Fitness);
            return (int)Math.Round(100 - effectiveModifiers.Sum(x => x.DrainValue));
        }
    }
}
