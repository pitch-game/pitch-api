using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Engine.Services;

namespace Pitch.Match.API.ApplicationCore.Engine
{
    public class MatchEngine : IMatchEngine
    {
        private readonly IActionService _actionService;
        private readonly IPossessionService _possessionService;

        public MatchEngine(IActionService actionService, IPossessionService possessionService)
        {
            _actionService = actionService;
            _possessionService = possessionService;
        }

        public Models.Match SimulateReentrant(Models.Match match)
        {
            match.AsAtElapsed(true);

            for (var minute = match.Elapsed; minute < Constants.MATCH_LENGTH_IN_MINUTES; minute++) //TODO atm its simulating the same minute again on reentrancy, is this right?
            {
                var inPossession = _possessionService.InPossession(match, out var notInPossession, out var homePossChance, out var awayPossChance);

                var action = _actionService.RollAction();
                if (action != null)
                {
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = _actionService.RollCard(affectedSquad, action, match.Minutes);
                    if (card != null)
                    {
                        var @event = action.SpawnEvent(card, affectedSquad.Id, match);
                        if (@event != null)
                        {
                            match.Minutes[minute].Events.Add(@event);
                        }
                    }
                }

                //TODO Fitness drain

                match.Minutes[minute].Stats = new MinuteStats(inPossession.Id, homePossChance, awayPossChance);
            }

            //extra time?
            return match;
        }
    }
}
