﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Events;

namespace Pitch.Match.API.ApplicationCore.Models.MatchResult
{
    public class MatchEventsWithMinute
    {
        public int Minute { get; set; }
        public IList<IEvent> Events { get; set; }
    }

    public class MatchResult
    {
        public MatchResult(Match.Match match)
        {
            SetCardLookup(match);

            var matchEventsWithMinutes = match.Minutes.Select((matchMinute, minute) => new MatchEventsWithMinute { Minute = minute, Events = matchMinute.Events }).ToList();

            SetCardStats(matchEventsWithMinutes);

            var homeTeamEvents = matchEventsWithMinutes.SelectMany(x => x.Events.Where(e => e.SquadId == match.HomeTeam.Squad.Id)).ToList();
            var awayTeamEvents = matchEventsWithMinutes.SelectMany(x => x.Events.Where(x => x.SquadId == match.AwayTeam.Squad.Id)).ToList();

            HomeStats = GetStats(match, homeTeamEvents, match.HomeTeam.Squad.Id);
            AwayStats = GetStats(match, awayTeamEvents, match.AwayTeam.Squad.Id);

            HomeResult = new Result
            {
                Score = homeTeamEvents.Count(x => x is Goal),
                Scorers = GetScorers(match, matchEventsWithMinutes, match.HomeTeam.Squad),
                Name = match.HomeTeam.Squad.Name
            };

            AwayResult = new Result
            {
                Score = awayTeamEvents.Count(x => x is Goal),
                Scorers = GetScorers(match, matchEventsWithMinutes, match.AwayTeam.Squad),
                Name = match.AwayTeam.Squad.Name
            };

            Timeline = matchEventsWithMinutes.SelectMany(x =>
            {
                return x.Events.Where(x => x.ShowInTimeline).Select((matchEvent, i) => new Event()
                {
                    Minute = x.Minute,
                    Name = matchEvent.Name,
                    CardId = matchEvent.CardId,
                    SquadName = match.HomeTeam.Squad.Id == matchEvent.SquadId ? match.HomeTeam.Squad.Name : match.AwayTeam.Squad.Name
                });
            }).OrderByDescending(x => x.Minute).ToList();

            SetLineups(match);

            Minute = match.Elapsed;
            Expired = match.HasFinished;
            ExpiredOn = match.HasFinished ? match.KickOff.AddMinutes(Constants.MatchLengthInMinutes) : (DateTime?)null;
        }

        private void SetCardLookup(Match.Match match)
        {
            var cards = new List<Card>();
            cards.AddRange(match.AwayTeam.Squad.Lineup.Values.SelectMany(x => x));
            cards.AddRange(match.AwayTeam.Squad.Subs);
            cards.AddRange(match.HomeTeam.Squad.Lineup.Values.SelectMany(x => x));
            cards.AddRange(match.HomeTeam.Squad.Subs);
            CardLookup = cards.Where(x => x != null).ToDictionary(x => x.Id.ToString(), x => x);
        }

        private void SetCardStats(IList<MatchEventsWithMinute> events)
        {
            foreach (var card in CardLookup)
            {
                card.Value.Goals = events.Sum(x => x.Events.Count(e => e.CardId == card.Value.Id && e is Goal));
                card.Value.YellowCards = events.Sum(x => x.Events.Count(e => e.CardId == card.Value.Id && e is YellowCard));
                card.Value.RedCards = events.Sum(x => x.Events.Count(e => e.CardId == card.Value.Id && e is RedCard));
            }
        }

        private void SetLineups(Match.Match match)
        {
            var homeSquad = new Squad(
                match.HomeTeam.Squad.Lineup.ToDictionary(x => x.Key, x => x.Value.Select(c => c.Id)),
                match.HomeTeam.Squad.Subs.Where(x => x != null).Select(x => x.Id).ToArray());
            var awaySquad = new Squad(
                match.AwayTeam.Squad.Lineup.ToDictionary(x => x.Key, x => x.Value.Select(c => c.Id)),
                match.AwayTeam.Squad.Subs.Where(x => x != null).Select(x => x.Id).ToArray());
            Lineup = new Lineup(homeSquad, awaySquad);
        }

        private static Stats GetStats(Match.Match match, IList<IEvent> homeTeamEvents, Guid teamId)
        {
            return new Stats()
            {
                Shots = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget), typeof(ShotOffTarget) }).Contains(x.GetType())),
                ShotsOnTarget = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget) }).Contains(x.GetType())),
                Possession = CalculatePossession(match, teamId),
                Fouls = homeTeamEvents.Count(x => (new Type[] { typeof(YellowCard), typeof(RedCard), typeof(Foul) }).Contains(x.GetType())),
                YellowCards = homeTeamEvents.Count(x => x is YellowCard),
                RedCards = homeTeamEvents.Count(x => x is RedCard)
            };
        }

        private static int CalculatePossession(Match.Match match, Guid teamId)
        {
            var stats = match.Minutes.Where(x => x.Stats != null).Select(x => x.Stats).ToList();
            if (!stats.Any()) return 0;
            return (int)Math.Round(stats.Count(x => x.SquadIdInPossession == teamId) / (double)stats.Count * 100);
        }

        private IEnumerable<string> GetScorers(Match.Match match, IList<MatchEventsWithMinute> events, ApplicationCore.Models.Squad team)
        {
            return events.SelectMany(x => 
            {
                return x.Events.Where(e => e is Goal && e.SquadId == team.Id).Cast<Goal>().Select(goal =>
                {
                    var player = CardLookup[goal.CardId.ToString()];
                    return $"{player.Name} {x.Minute}'";
                });
            });
        }

        public int Minute { get; set; }

        public Result HomeResult { get; set; }
        public Result AwayResult { get; set; }

        public Stats HomeStats { get; set; }
        public Stats AwayStats { get; set; }

        public Lineup Lineup { get; set; }

        public IList<Event> Timeline { get; set; }

        public bool Expired { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public IDictionary<string, Card> CardLookup { get; set; }
    }
}
