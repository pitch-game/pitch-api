using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Actions;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Providers;

namespace Pitch.Match.Engine.Services
{
    public interface IActionService
    {
        IAction RollAction();
        Card RollCard(Squad team, IAction action, MatchMinute[] minutes);
    }

    public class ActionService : IActionService
    {
        private readonly IEnumerable<IAction> _actions;
        private readonly IRandomnessProvider _randomnessProvider;

        public ActionService(IEnumerable<IAction> actions, IRandomnessProvider randomnessProvider)
        {
            _actions = actions;
            _randomnessProvider = randomnessProvider;
        }

        public IAction RollAction()
        {
            return ChanceHelper.PercentBase100Chance(_actions, x => x.ChancePerMinute);
        }

        public Card RollCard(Squad team, IAction action, MatchMinute[] minutes)
        {
            PositionalArea positionalArea = ChanceHelper.PercentBase100Chance(action.PositionalChance, x => x.Value).Key;

            var sentOffCardIds = minutes.SelectMany(x => x.Events).Where(x => x.Type == EventType.RedCard).Select(x => x.CardId);

            if (!team.Lineup.ContainsKey(positionalArea.ToString())) return null;
            var cards = team.Lineup[positionalArea.ToString()].Where(x => x != null && !sentOffCardIds.Contains(x.Id)).ToList();

            int r = _randomnessProvider.Next(cards.Count);
            return cards.ElementAtOrDefault(r); //returns null TODO fix rolling position with 0 cards due to sending off
        }
    }
}
