using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.ApplicationCore.Models.MatchResult {
    public class Squad  {
        public IDictionary<string, IEnumerable<Guid>> Lineup { get; set; }
        public Guid[] Subs { get; set; }
    }
}