using Pitch.Match.Api.ApplicationCore.Engine.Actions;
using Pitch.Match.Api.ApplicationCore.Engine.Helpers;
using Pitch.Match.Api.ApplicationCore.Models.Match;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.ApplicationCore.Engine
{
    public class MatchEngine : IMatchEngine
    {
        private readonly IEnumerable<IAction> _actions;

        public MatchEngine(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        public Models.Match.Match SimulateReentrant(Models.Match.Match match)
        {
            match.Events = match.Events.Where(x => x.Minute <= match.Duration).ToList();
            match.Statistics = match.Statistics.Where(x => x.Minute <= match.Duration).ToList();

            for (int minute = match.Duration; minute < Constants.MATCH_LENGTH_IN_MINUTES; minute++) //TODO atm its simulating the same minute again on reentrancy, is this right?
            {
                int homePossChance;
                int awayPossChance;

                Squad notInPossession;
                Squad inPossession = PossessionHelper.InPossession(match, out notInPossession, out homePossChance, out awayPossChance);

                IAction action = ActionHelper.RollAction(_actions);
                if (action != null)
                {
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = ActionHelper.RollCard(affectedSquad, action, match.Events);
                    if (card == null) //TODO cancel if couldnt get a card
                        continue;

                    var @event = action.SpawnEvent(card, affectedSquad.Id, minute, match);
                    if (@event != null)
                        match.Events.Add(@event);
                }

                FitnessHelper.Drain(inPossession, notInPossession);

                match.Statistics.Add(new MinuteStats(minute, inPossession.Id, homePossChance, awayPossChance));
            }

            //extra time?
            return match;
        }
    }
}
