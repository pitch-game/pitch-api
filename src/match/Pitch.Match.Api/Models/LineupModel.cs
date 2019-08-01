using Pitch.Match.Api.ApplicationCore.Models.Match;
using System.Collections.Generic;

namespace Pitch.Match.Api.Models
{
    public class LineupModel
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
