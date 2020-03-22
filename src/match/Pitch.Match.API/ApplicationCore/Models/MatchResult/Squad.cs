using System;
using System.Collections.Generic;

namespace Pitch.Match.API.ApplicationCore.Models.MatchResult {
    public class Squad  {
        public Squad(IDictionary<string, IEnumerable<Guid>> lineup, Guid[] subs)
        {
            Lineup = lineup;
            Subs = subs;
        }

        public IDictionary<string, IEnumerable<Guid>> Lineup { get; set; }
        public Guid[] Subs { get; set; }
    }
}