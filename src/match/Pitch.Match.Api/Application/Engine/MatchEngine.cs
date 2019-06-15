using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Services
{
    public class MatchEngine : IMatchEngine
    {
        private const int MATCH_LENGTH_IN_MINUTES = 90;
        private readonly IEnumerable<IAction> _actions;

        public MatchEngine(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        //Reentrant
        public Models.Match SimulateReentrant(Models.Match match)
        {
            var simulateFrom = match.SimulatedMinute;
            for (int i = simulateFrom; i < MATCH_LENGTH_IN_MINUTES; i++)
            {
                //which team has possession (midfield * 1 + def * 0.5 + st * 0.5 + gk * 0.1) rating + fitness
                Random rand = new Random();
                var inPossession = (rand.Next(0, 2) != 0) ? match.Team1 : match.Team2;

                Random random = new Random();
                int randomNumber = random.Next(0, 100);

                decimal accumulatedProbability = 0;
                var actions = _actions.ToList();
                IAction action = null;
                for (int p = 0; p < actions.Count; p++)
                {
                    accumulatedProbability += actions[p].ChancePerMinute;
                    if (randomNumber <= accumulatedProbability * 100)
                    {
                        action = actions[p];
                        break;
                    }
                }

                if (action != null)
                {
                    var card = GetCardForEvent(inPossession, action);
                    match.Events.Add(GetEventFromAction(card, action, inPossession.Id, i)); //TODO Map to event
                }

                //no event occurs
                //decrease fitness
            }

            //extra time?

            return match;
        }

        private Card GetCardForEvent(Squad team, IAction action)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            var accumulatedProbability = 0m;
            PositionalArea positionalArea = 0;
            foreach (var position in action.PositionalChance)
            {
                accumulatedProbability += position.Value;
                if (randomNumber <= accumulatedProbability * 100)
                {
                    positionalArea = position.Key;
                    break;
                }
            }

            var cards = team.Lineup[positionalArea].ToList();
            var rnd = new Random();
            int r = rnd.Next(cards.Count);
            return cards[r];
        }

        private IMatchEvent GetEventFromAction(Card card, IAction action, Guid squadId, int minute)
        {
            switch (action)
            {
                case Shot shot:
                    //on target vs goal = gk vs att
                    Random rand = new Random();
                    return (rand.Next(0, 2) != 0) ? (IMatchEvent)new ShotOnTarget()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    } : (IMatchEvent)new Application.Engine.Events.Goal()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    };
                case Application.Engine.Action.YellowCard yCard:
                    return new Application.Engine.Events.YellowCard()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    };
                case Application.Engine.Action.RedCard rCard:
                    return new Application.Engine.Events.RedCard()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    };
                default:
                    return null;
            }
        }
    }
}
