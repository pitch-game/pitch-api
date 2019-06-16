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
            HomeResult = new Result
            {
                Score = match.Events.Where(x => x.SquadId == match.HomeTeam.Id && x.GetType() == typeof(Goal)).Count(),
                Scorers = match.HomeTeam.Lineup.SelectMany(x => x.Value).Where(x => match.Events.Where(m => m.SquadId == match.HomeTeam.Id && m.GetType() == typeof(Goal)).Select(c => c.CardId).Contains(x.Id)).Select(x => $"{x.Name}").ToList()
            };
            AwayResult = new Result
            {
                Score = match.Events.Where(x => x.SquadId == match.AwayTeam.Id && x.GetType() == typeof(Goal)).Count(),
                Scorers = match.AwayTeam.Lineup.SelectMany(x => x.Value).Where(x => match.Events.Where(m => m.SquadId == match.AwayTeam.Id && m.GetType() == typeof(Goal)).Select(c => c.CardId).Contains(x.Id)).Select(x => $"{x.Name}").ToList()
            };

            HomePossessionPercent = (int)Math.Round(((double)match.Statistics.Count(x => x.SquadIdInPossession == match.HomeTeam.Id) / (double)match.Statistics.Count()) * 100);
            AwayPossessionPercent = (int)Math.Round(((double)match.Statistics.Count(x => x.SquadIdInPossession == match.AwayTeam.Id) / (double)match.Statistics.Count()) * 100);
        }

        public int Minute { get; set; }

        public Result HomeResult { get; set; }
        public Result AwayResult { get; set; }

        public int HomePossessionPercent { get; set; }
        public int AwayPossessionPercent { get; set; }

    }

    public class Result
    {
        public int Score { get; set; }
        public IEnumerable<string> Scorers { get; set; }
    }
}
