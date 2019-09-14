using Pitch.Match.API.ApplicationCore.Models;
using System.Collections.Generic;

namespace Pitch.Match.API.Models
{
    public class LineupModel
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
