using System.Collections.Generic;

namespace Pitch.Match.API.ApplicationCore.Models
{
    public class Lineup
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
