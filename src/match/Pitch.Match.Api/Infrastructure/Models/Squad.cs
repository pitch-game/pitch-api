using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Infrastructure.Models
{
    public class Squad
    {
        public Squad()
        {
            Lineup = new Dictionary<string, IEnumerable<Card>>();
            Subs = new Card[6];
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IDictionary<string, IEnumerable<Card>> Lineup { get; set; }
        public Card[] Subs { get; set; }
    }
}
