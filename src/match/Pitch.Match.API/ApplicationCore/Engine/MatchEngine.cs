using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Engine.Services;

namespace Pitch.Match.API.ApplicationCore.Engine
{
    public class MatchEngine : IMatchEngine
    {
        private readonly IActionService _actionService;

        public MatchEngine(IActionService actionService)
        {
            _actionService = actionService;
        }

        public Models.Match SimulateReentrant(Models.Match match)
        {
            match.AsAtElapsed(true);

            for (int minute = match.Elapsed; minute < Constants.MATCH_LENGTH_IN_MINUTES; minute++) //TODO atm its simulating the same minute again on reentrancy, is this right?
            {
                Squad inPossession = PossessionHelper.InPossession(match, out var notInPossession, out var homePossChance, out var awayPossChance);

                IAction action = _actionService.RollAction();
                if (action != null)
                {
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = _actionService.RollCard(affectedSquad, action, match.Events);
                    if (card != null)
                    {
                        var @event = action.SpawnEvent(card, affectedSquad.Id, minute, match);
                        if (@event != null)
                            match.Minutes[minute].Events.Add(@event);
                    }
                }

                //TODO Fitness drain

                match.Minutes[minute].Stats = new MinuteStats(minute, inPossession.Id, homePossChance, awayPossChance);
            }

            //extra time?
            return match;
        }
    }
}
