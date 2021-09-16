using System.Collections.Generic;

namespace Pitch.Match.Api.ApplicationCore.Models.MatchResult
{
    public class Result
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public IEnumerable<string> Scorers { get; set; }
    }
}