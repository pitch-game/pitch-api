using Pitch.Match.Api.Application.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Models
{
    public class MatchResult
    {
        public MatchResult(Models.Match match)
        {
            var homeTeamEvents = match.Events.Where(x => x.SquadId == match.HomeTeam.Squad.Id);
            var awayTeamEvents = match.Events.Where(x => x.SquadId == match.AwayTeam.Squad.Id);

            HomeStats = GetStats(match, homeTeamEvents, match.HomeTeam.Squad.Id);
            AwayStats = GetStats(match, awayTeamEvents, match.AwayTeam.Squad.Id);

            HomeResult = new Result
            {
                Score = homeTeamEvents.Count(x => x.GetType() == typeof(Goal)),
                Scorers = GetScorers(match, homeTeamEvents, match.HomeTeam.Squad),
                Name = match.HomeTeam.Squad.Name
            };

            AwayResult = new Result
            {
                Score = awayTeamEvents.Count(x => x.GetType() == typeof(Goal)),
                Scorers = GetScorers(match, awayTeamEvents, match.AwayTeam.Squad),
                Name = match.AwayTeam.Squad.Name
            };

            var cards = match.HomeTeam.Squad.Lineup.SelectMany(x => x.Value).Concat(match.AwayTeam.Squad.Lineup.SelectMany(x => x.Value));
            Events = match.Events.Where(x => x.ShowInTimeline).OrderByDescending(x => x.Minute).Select(x => new Event()
            {
                Minute = x.Minute,
                Name = x.Name,
                Card = cards.FirstOrDefault(c => c.Id == x.CardId),
                SquadName = match.HomeTeam.Squad.Id == x.SquadId ? match.HomeTeam.Squad.Name : match.AwayTeam.Squad.Name, //TODO sending repeated data
                CardId = x.CardId
            }).ToList();

            Minute = match.Duration;
            Expired = match.IsOver;
            ExpiredOn = match.IsOver ? match.KickOff.AddMinutes(90) : (DateTime?)null;
        }

        private static Stats GetStats(Match match, IEnumerable<IEvent> homeTeamEvents, Guid teamId)
        {
            return new Stats()
            {
                Shots = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget), typeof(ShotOffTarget) }).Contains(x.GetType())),
                ShotsOnTarget = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget) }).Contains(x.GetType())),
                Possession = CalculatePossession(match, teamId),
                Fouls = homeTeamEvents.Count(x => (new Type[] { typeof(YellowCard), typeof(RedCard), typeof(Foul) }).Contains(x.GetType())),
                YellowCards = homeTeamEvents.Count(x => x.GetType() == typeof(YellowCard)),
                RedCards = homeTeamEvents.Count(x => x.GetType() == typeof(RedCard))
            };
        }

        private static int CalculatePossession(Match match, Guid teamId)
        {
            if (match.Statistics.Count() == 0) return 0;
            return (int)Math.Round(((double)match.Statistics.Count(x => x.SquadIdInPossession == teamId) / (double)match.Statistics.Count()) * 100);
        }

        private static IEnumerable<string> GetScorers(Match match, IEnumerable<IEvent> events, Squad team)
        {
            var scorers = new List<string>();
            var goals = events.Where(x => x.GetType() == typeof(Goal)).Cast<Goal>();
            var playerCards = team.Lineup.SelectMany(x => x.Value);
            foreach (var goal in goals)
            {
                var player = playerCards.FirstOrDefault(x => x.Id == goal.CardId);
                scorers.Add($"{player.Name} {goal.Minute}'");
            }
            return scorers;
        }

        public int Minute { get; set; }

        public Result HomeResult { get; set; }
        public Result AwayResult { get; set; }

        public Stats HomeStats { get; set; }
        public Stats AwayStats { get; set; }

        //TODO Rename to timeline?
        public IList<Event> Events { get; set; }

        public bool Expired { get; set; }

        public DateTime? ExpiredOn { get; set; }
    }

    public class Stats
    {
        public int Shots { get; set; }
        public int ShotsOnTarget { get; set; }
        public int Possession { get; set; }
        public int Fouls { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }

    public class Result
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public IEnumerable<string> Scorers { get; set; }
    }

    public class Event
    {
        public int Minute { get; set; }
        public string Name { get; set; }
        public Card Card { get; set; }
        public string SquadName { get; set; }
        public Guid CardId { get; set; }
    }
}
