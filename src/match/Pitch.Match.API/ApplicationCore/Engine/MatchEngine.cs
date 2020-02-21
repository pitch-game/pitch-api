using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Engine.Services;

namespace Pitch.Match.API.ApplicationCore.Engine
{
    public interface IMatchEngine
    {
        Models.Match Simulate(Models.Match match);
    }

    public class MatchEngine : IMatchEngine
    {
        private readonly IActionService _actionService;
        private readonly IPossessionService _possessionService;

        public MatchEngine(IActionService actionService, IPossessionService possessionService)
        {
            _actionService = actionService;
            _possessionService = possessionService;
        }

        /// <summary>
        /// Reentrant method for simulating a match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public Models.Match Simulate(Models.Match match)
        {
            match.AsAtElapsed(true);

            for (var minute = match.Elapsed; minute < Constants.MatchLengthInMinutes; minute++) //TODO atm its simulating the same minute again on reentrancy, is this right?
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
                //match.Tick(); TODO fitness drain

                match.Minutes[minute].Stats = new MinuteStats(inPossession.Id, homePossChance, awayPossChance);
            }

            return match;
        }
    }
}
