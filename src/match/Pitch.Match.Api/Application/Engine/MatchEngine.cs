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
            //remove events after simulateFrom, but does that work with sending offs etc
            //bool forceReRoll = true;
            //while (forceReRoll) {
            //    forceReRoll = false;

                for (int minute = simulateFromMinute; minute < MATCH_LENGTH_IN_MINUTES; minute++)
                {
                    int homePossChance;
                    int awayPossChance;

                    Squad notInPossession;
                    Squad inPossession = PossessionHelper.InPossession(match, out notInPossession, out homePossChance, out awayPossChance);

                    IAction action = ActionHelper.RollAction(_actions);
                    if (action != null)
                    {
                        var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                        var card = ActionHelper.RollCard(affectedSquad, action);
                        if (card == null) //TODO cancel if couldnt get a card
                            continue;

                        bool forceReRoll;
                        var @event = action.SpawnEvent(card, affectedSquad.Id, minute, match, out forceReRoll);
                        if(@event != null)
                            match.Events.Add(@event);

                        //if (forceReRoll)
                        //{
                        //    simulateFromMinute = minute + 1; //TODO simulate the same minute again?
                        //    break;
                        //}
                    }

                    FitnessHelper.Drain(inPossession, notInPossession);

                    match.Statistics.Add(new MinuteStats(minute, inPossession.Id, homePossChance, awayPossChance));
                }
            //}

            //extra time?
            return match;
        }
    }
}
