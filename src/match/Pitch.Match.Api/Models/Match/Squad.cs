using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Models
{
    public class Squad
    {
        public Guid Id { get; set; }
        public IDictionary<PositionalArea, IEnumerable<Card>> Lineup { get; set; }
    }

    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public int Fitness { get; set; }
    }
}
