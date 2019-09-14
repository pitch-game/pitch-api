using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.API.ApplicationCore.Engine.Helpers
{
    public static class ActionHelper
    {
        public static IAction RollAction(IEnumerable<IAction> _actions)
        {
            return ChanceHelper.PercentBase100Chance(_actions, x => x.ChancePerMinute);
        }

        public static Card RollCard(Squad team, IAction action, IEnumerable<IEvent> events)
        {
            PositionalArea positionalArea = ChanceHelper.PercentBase100Chance(action.PositionalChance, x => x.Value).Key;

            var sentOffCardIds = events.Where(x => x.GetType() == typeof(RedCard)).Select(x => x.CardId);
            var cards = team.Lineup[positionalArea.ToString()].Where(x => x != null && !sentOffCardIds.Contains(x.Id)).ToList();

            var rnd = new Random();
            int r = rnd.Next(cards.Count);
            return cards.ElementAtOrDefault(r); //returns null TODO fix rolling position with 0 cards due to sending off
        }
    }
}
