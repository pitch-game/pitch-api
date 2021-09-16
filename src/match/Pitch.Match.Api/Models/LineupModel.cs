using Pitch.Match.Api.ApplicationCore.Models;
using System.Collections.Generic;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.Models
{
    public class LineupModel
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
