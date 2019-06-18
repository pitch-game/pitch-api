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
            var homeTeamEvents = match.Events.Where(x => x.SquadId == match.HomeTeam.Id);
            var awayTeamEvents = match.Events.Where(x => x.SquadId == match.AwayTeam.Id);

            HomeStats = GetStats(match, homeTeamEvents, match.HomeTeam.Id);
            AwayStats = GetStats(match, awayTeamEvents, match.AwayTeam.Id);

            HomeResult = new Result
            {
                Score = homeTeamEvents.Count(x => x.GetType() == typeof(Goal)),
                Scorers = GetScorers(match, homeTeamEvents, match.HomeTeam)
            };

            AwayResult = new Result
            {
                Score = awayTeamEvents.Count(x => x.GetType() == typeof(Goal)),
                Scorers = GetScorers(match, awayTeamEvents, match.AwayTeam)
            };

            Events = match.Events;
        }

        private static Stats GetStats(Match match, IEnumerable<IEvent> homeTeamEvents, Guid teamId)
        {
            return new Stats()
            {
                Shots = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget), typeof(ShotOffTarget) }).Contains(x.GetType())),
                ShotsOnTarget = homeTeamEvents.Count(x => (new Type[] { typeof(Goal), typeof(ShotOnTarget) }).Contains(x.GetType())),
                Possession = (int)Math.Round(((double)match.Statistics.Count(x => x.SquadIdInPossession == teamId) / (double)match.Statistics.Count()) * 100),
                Fouls = homeTeamEvents.Count(x => (new Type[] { typeof(YellowCard), typeof(RedCard) }).Contains(x.GetType())), //TODO Foul event
                YellowCards = homeTeamEvents.Count(x => x.GetType() == typeof(YellowCard)),
                RedCards = homeTeamEvents.Count(x => x.GetType() == typeof(RedCard))
            };
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

        public IList<IEvent> Events { get; set; }

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
        public int Score { get; set; }
        public IEnumerable<string> Scorers { get; set; }
    }
}
