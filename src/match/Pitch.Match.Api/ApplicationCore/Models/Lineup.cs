using System.Collections.Generic;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.ApplicationCore.Models
{
    public class Lineup
    {
        public IList<Card> Active { get; set; }
        public Card[] Subs { get; set; }
    }
}
