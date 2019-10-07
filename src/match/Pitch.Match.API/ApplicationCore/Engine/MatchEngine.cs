using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Engine.Helpers;
using Pitch.Match.API.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.API.ApplicationCore.Engine
{
    public class MatchEngine : IMatchEngine
    {
        private readonly IEnumerable<IAction> _actions;

        public MatchEngine(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        public Models.Match SimulateReentrant(Models.Match match)
        {
            match.AsAtElapsed(true);

            for (int minute = match.Elapsed; minute < Constants.MATCH_LENGTH_IN_MINUTES; minute++) //TODO atm its simulating the same minute again on reentrancy, is this right?
            {
                Squad inPossession = PossessionHelper.InPossession(match, out var notInPossession, out var homePossChance, out var awayPossChance);

                IAction action = ActionHelper.RollAction(_actions);
                if (action != null)
                {
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = ActionHelper.RollCard(affectedSquad, action, match.Events);
                    if (card != null)
                    {
                        var @event = action.SpawnEvent(card, affectedSquad.Id, minute, match);
                        if (@event != null)
                            match.Events.Add(@event);
                    }
                }

                FitnessHelper.Drain(inPossession, notInPossession);

                match.Statistics.Add(new MinuteStats(minute, inPossession.Id, homePossChance, awayPossChance));
            }

            //extra time?
            return match;
        }
    }
}
