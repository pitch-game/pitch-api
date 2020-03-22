using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Events;

namespace Pitch.Match.API.ApplicationCore.Models.MatchResult
{
    public class MatchResult
    {
        public MatchResult(Match.Match match)
        {
            var matchEvents = match.Minutes.SelectMany(x => x.Events).ToList();

            var homeTeamEvents = matchEvents.Where(x => x.SquadId == match.HomeTeam.Squad.Id).ToList();
            var awayTeamEvents = matchEvents.Where(x => x.SquadId == match.AwayTeam.Squad.Id).ToList();

            HomeStats = GetStats(match, homeTeamEvents, match.HomeTeam.Squad.Id);
            AwayStats = GetStats(match, awayTeamEvents, match.AwayTeam.Squad.Id);

            HomeResult = new Result
            {
                Score = homeTeamEvents.Count(x => x is Goal),
                Scorers = GetScorers(match, homeTeamEvents, match.HomeTeam.Squad),
                Name = match.HomeTeam.Squad.Name
            };

            AwayResult = new Result
            {
                Score = awayTeamEvents.Count(x => x is Goal),
                Scorers = GetScorers(match, awayTeamEvents, match.AwayTeam.Squad),
                Name = match.AwayTeam.Squad.Name
            };

            Timeline = matchEvents.Where(x => x.ShowInTimeline).Select((matchEvent, i) => new Event()
            {
                Minute = i, //TODO fix get actual minute
                Name = matchEvent.Name,
                CardId = matchEvent.CardId,
                SquadName = match.HomeTeam.Squad.Id == matchEvent.SquadId
                        ? match.HomeTeam.Squad.Name
                        : match.AwayTeam.Squad.Name, //TODO sending repeated data
            }).ToList();

            var homeSquad = new ApplicationCore.Models.MatchResult.Squad(match.HomeTeam.Squad.Lineup.ToDictionary(x => x.Key, x => x.Value.Select(c => c.Id)), match.HomeTeam.Squad.Subs.Select(x => x.Id).ToArray());
            var awaySquad = new ApplicationCore.Models.MatchResult.Squad(match.AwayTeam.Squad.Lineup.ToDictionary(x => x.Key, x => x.Value.Select(c => c.Id)), match.AwayTeam.Squad.Subs.Select(x => x.Id).ToArray());
            Lineup = new ApplicationCore.Models.MatchResult.Lineup(homeSquad, awaySquad);

            var cards = new List<Card>();
            cards.AddRange(match.AwayTeam.Squad.Lineup.Values.SelectMany(x => x));
            cards.AddRange(match.AwayTeam.Squad.Subs);
            cards.AddRange(match.HomeTeam.Squad.Lineup.Values.SelectMany(x => x));
            cards.AddRange(match.HomeTeam.Squad.Subs);
            CardLookup = cards.ToDictionary(x => x.Id, x => x);

            Minute = match.Elapsed;
            Expired = match.HasFinished;
            ExpiredOn = match.HasFinished ? match.KickOff.AddMinutes(Constants.MatchLengthInMinutes) : (DateTime?)null;
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

        private IEnumerable<string> GetScorers(Match.Match match, IEnumerable<IEvent> events, ApplicationCore.Models.Squad team)
        {
            var scorers = new List<string>();
            var goals = events.Where(x => x is Goal).Cast<Goal>();
            foreach (var goal in goals)
            {
                var player = CardLookup[goal.CardId];
                scorers.Add($"{player.Name} {0}'");
            }
            return scorers;
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

        public IDictionary<Guid, Card> CardLookup { get; set; }
    }
}
