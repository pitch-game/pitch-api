using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Application.Engine.Helpers;
using Pitch.Match.Api.Models;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.Engine
{
    public class MatchEngine : IMatchEngine
    {
        private const int MATCH_LENGTH_IN_MINUTES = 90;

        private readonly IEnumerable<IAction> _actions;

        public MatchEngine(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        public Models.Match SimulateReentrant(Models.Match match, int simulateFromMinute = 0)
        {
            //remove events after simulateFrom
            for (int minute = simulateFromMinute; minute < MATCH_LENGTH_IN_MINUTES; minute++)
            {
                Squad notInPossession;
                Squad inPossession = PossessionHelper.InPossession(match, out notInPossession);

                IAction action = ActionHelper.RollAction(_actions);
                if (action != null)
                {
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = ActionHelper.RollCard(affectedSquad, action);

                    var @event = action.SpawnEvent(card, affectedSquad.Id, minute, match);
                    match.Events.Add(@event);
                }

                FitnessHelper.Drain(inPossession, notInPossession);

                match.Statistics.Add(new MinuteStats(minute, inPossession.Id));
            }

            //extra time?
            return match;
        }
    }
}
