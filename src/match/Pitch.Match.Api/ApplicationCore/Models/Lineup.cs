using System.Collections.Generic;

namespace Pitch.Match.Api.ApplicationCore.Models
{
    public class Lineup
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
