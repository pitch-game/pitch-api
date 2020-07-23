using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Engine.Services;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.ApplicationCore.Engine
{
    public interface IMatchEngine
    {
        Models.Match.Match Simulate(Models.Match.Match match);
    }

    public class MatchEngine : IMatchEngine
    {
        private readonly IActionService _actionService;
        private readonly IPossessionService _possessionService;
        private readonly IFitnessDrainService _fitnessDrainService;
        private readonly ICalculatedCardStatService _calculatedCardStatService;

        public MatchEngine(IActionService actionService, IPossessionService possessionService, IFitnessDrainService fitnessDrainService, ICalculatedCardStatService calculatedCardStatService)
        {
            _actionService = actionService;
            _possessionService = possessionService;
            _fitnessDrainService = fitnessDrainService;
            _calculatedCardStatService = calculatedCardStatService;
        }

        /// <summary>
        /// Reentrant method for simulating a match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public Models.Match.Match Simulate(Models.Match.Match match)
        {
            match.AsAtElapsed(true);

            for (var minute = match.Elapsed; minute < Constants.MatchLengthInMinutes; minute++)
            {
                SimulateMinute(match, minute);
            }

            return match;
        }

        private void SimulateMinute(Models.Match.Match match, int minute)
        {
            SetCalculatedStats(match);

            var inPossession = _possessionService.InPossession(match, out var notInPossession, out var homePossChance,
                out var awayPossChance);

            var action = _actionService.RollAction();
            if (action != null)
            {
                var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                var affectedCard = _actionService.RollCard(affectedSquad, action, match.Minutes);
                if (affectedCard != null)
                {
                    var @event = action.SpawnEvent(affectedCard, affectedSquad.Id, match);
                    if (@event != null)
                    {
                        match.Minutes[minute].Events.Add(@event);
                    }
                }
            }

            ApplyModifiers(match, minute);

            AssignStats(match, minute, inPossession, homePossChance, awayPossChance);
        }

        private static void AssignStats(Models.Match.Match match, int minute, Squad inPossession, int homePossChance, int awayPossChance)
        {
            match.Minutes[minute].Stats = new MinuteStats(inPossession.Id, homePossChance, awayPossChance);
        }

        private void ApplyModifiers(Models.Match.Match match, int minute)
        {
            _fitnessDrainService.Drain(match, minute);
        }

        private void SetCalculatedStats(Models.Match.Match match)
        {
            _calculatedCardStatService.Set(match);
        }
    }
}
