using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Application.Engine.Helpers
{
    public static class ActionHelper
    {
        public static IAction RollAction(IEnumerable<IAction> _actions)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            decimal accumulatedProbability = 0;
            var actions = _actions.ToList();
            IAction action = null;
            for (int p = 0; p < actions.Count; p++)
            {
                accumulatedProbability += actions[p].ChancePerMinute;
                if (randomNumber <= accumulatedProbability * 100)
                {
                    action = actions[p];
                    break;
                }
            }

            return action;
        }

        public static Card RollCard(Squad team, IAction action)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            var accumulatedProbability = 0m;
            PositionalArea positionalArea = 0;
            foreach (var position in action.PositionalChance)
            {
                accumulatedProbability += position.Value;
                if (randomNumber <= accumulatedProbability * 100)
                {
                    positionalArea = position.Key;
                    break;
                }
            }

            var cards = team.Lineup[positionalArea].ToList();
            var rnd = new Random();
            int r = rnd.Next(cards.Count);
            return cards[r];
        }
    }
}
