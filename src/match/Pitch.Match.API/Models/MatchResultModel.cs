using System;
using System.Collections.Generic;
using Pitch.Match.Api.ApplicationCore.Models;
using Pitch.Match.Api.ApplicationCore.Models.MatchResult;
using Pitch.Match.Engine.Models;
using Lineup = Pitch.Match.Api.ApplicationCore.Models.MatchResult.Lineup;

namespace Pitch.Match.Api.Models
{
    public class MatchResultModel
    {
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
