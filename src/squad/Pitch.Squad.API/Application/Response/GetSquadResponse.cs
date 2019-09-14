using Pitch.Squad.API.Application.Models;
using Pitch.Squad.API.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Squad.API.Application.Response
{
    public class GetSquadResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IDictionary<string, Card> Lineup { get; set; }
        public Card[] Subs { get; set; }
    }
}
