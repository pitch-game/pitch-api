using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.ApplicationCore.Engine.Services
{
    public interface ICalculatedCardStatService
    {
        void Set(Models.Match.Match match);
    }

    public class CalculatedCardStatService : ICalculatedCardStatService
    {
        private const int FitnessUpperBound = 100;

        public void Set(Models.Match.Match match)
        {
            var modifiers = match.Minutes.SelectMany(x => x.Modifiers);
            SetFitness(modifiers, match.HomeTeam.Squad);
            SetFitness(modifiers, match.AwayTeam.Squad);
        }

        private void SetFitness(IEnumerable<Modifier> modifiers, Squad squad)
        {
            foreach (var position in squad.Lineup)
            {
                foreach (var card in position.Value)
                {
                    card.Fitness = Fitness(modifiers, card.Id);
                }
            }

            foreach (var card in squad.Subs.Where(x => x != null))
            {
                card.Fitness = Fitness(modifiers, card.Id);
            }
        }

        private static int Fitness(IEnumerable<Modifier> modifiers, Guid cardId)
        {
            return (int)Math.Round(FitnessUpperBound - EffectiveModifiers(modifiers, cardId, ModifierType.Fitness).Sum(x => x.DrainValue));
        }

        private static IEnumerable<Modifier> EffectiveModifiers(IEnumerable<Modifier> modifiers, Guid cardId, ModifierType type)
        {
            return modifiers.Where(x => x.CardId == cardId && x.Type == type);
        }
    }
}
