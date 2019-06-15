using Pitch.Match.Api.Application.Engine.Events;
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
                Score = match.Events.Where(x => x.SquadId == match.Team1.Id && x.GetType() == typeof(Goal)).Count(),
                Scorers = match.Team1.Lineup.SelectMany(x => x.Value).Where(x => match.Events.Where(m => m.SquadId == match.Team1.Id && m.GetType() == typeof(Goal)).Select(c => c.CardId).Contains(x.Id)).Select(x => $"{x.Name}").ToList()
            };
            AwayResult = new Result
            {
                Score = match.Events.Where(x => x.SquadId == match.Team2.Id && x.GetType() == typeof(Goal)).Count(),
                Scorers = match.Team2.Lineup.SelectMany(x => x.Value).Where(x => match.Events.Where(m => m.SquadId == match.Team2.Id && m.GetType() == typeof(Goal)).Select(c => c.CardId).Contains(x.Id)).Select(x => $"{x.Name}").ToList()
            };
        }

        public Result HomeResult { get; set; }
        public Result AwayResult { get; set; }
    }

    public class Result
    {
        public int Score { get; set; }
        public IEnumerable<string> Scorers { get; set; }
    }
}
