using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            for (int i = simulateFromMinute; i <= MATCH_LENGTH_IN_MINUTES; i++)
            {
                Squad notInPossession = null;
                Squad inPossession = InPossession(match, out notInPossession);

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
                    var affectedSquad = action.AffectsTeamInPossession ? inPossession : notInPossession;
                    var card = GetCardForEvent(affectedSquad, action);
                    match.Events.Add(GetEventFromAction(card, action, affectedSquad.Id, i)); //TODO Map to event
                }

                //no event occurs
                //decrease fitness
                DrainFitness(inPossession, notInPossession);
                match.Statistics.Add(new MinuteStats(i, inPossession.Id));
            }

            //extra time?

            return match;
        }

        private void DrainFitness(Squad inPossession, Squad notInPossession)
        {
            //fitness drains slightly faster for the team out of possession
            //foreach (var item in homeSquad.Lineup)
            //{

            //}
        }

        private Squad InPossession(Models.Match match, out Squad notInPossession)
        {
            var team1Chance = PossessionChance(match.HomeTeam);
            var team2Chance = PossessionChance(match.AwayTeam);

            var difference = Math.Abs(team1Chance - team2Chance);

            var team1Percent = (int)Math.Round(100 - (((double)difference / (double)team1Chance) * 100));
            var team2Percent = (int)Math.Round(100 - (((double)difference / (double)team2Chance) * 100));

            var accumulatedWeight = team1Percent + team2Percent;

            var team1InPossession = false;

            var rand = new Random();
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= team1Percent)
            {
                team1InPossession = true;
            }

            if (team1InPossession)
            {
                notInPossession = match.AwayTeam;
                return match.HomeTeam;
            }
            else
            {
                notInPossession = match.HomeTeam;
                return match.AwayTeam;
            }
            //record possession stats
        }

        private int PossessionChance(Squad squad)
        {
            return (int)Math.Round((CurrentRating(PositionalArea.GK, squad) * 0.1) +
                (CurrentRating(PositionalArea.DEF, squad) * 0.5) +
                (CurrentRating(PositionalArea.MID, squad) * 1) +
                (CurrentRating(PositionalArea.ATT, squad) * 0.5));
        }

        private int CurrentRating(PositionalArea positionalArea, Squad squad)
        {
            var players = squad.Lineup.Where(x => x.Key == positionalArea).SelectMany(x => x.Value).ToList();
            return (int)Math.Round((players.Sum(x => x.Rating * 0.7) + players.Sum(x => x.Fitness * 0.3)) / players.Count);
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
                    //on target / off target = st vs def
                    //on target / goal = st vs gk
                    Random rand = new Random();
                    return rand.Next(0, 2) != 0 ? new ShotOnTarget()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    } : (IMatchEvent)new Goal()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    };
                case Action.YellowCard yCard:
                    return new Events.YellowCard()
                    {
                        CardId = card.Id,
                        SquadId = squadId,
                        Minute = minute
                    };
                case Action.RedCard rCard:
                    return new Events.RedCard()
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
