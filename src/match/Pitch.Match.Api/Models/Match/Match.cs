using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Models
{
    public class Match
    {
        public Match()
        {
            Events = new List<IMatchEvent>();
        }
        public Guid Id { get; set; }
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }

        public Squad Team1 { get; set; }
        public Squad Team2 { get; set; }

        public DateTime KickOff { get; set; }

        public int SimulatedMinute { get; set; }

        public IList<IMatchEvent> Events { get; set; }
    }
}
