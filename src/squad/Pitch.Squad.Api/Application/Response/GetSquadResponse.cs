using Pitch.Squad.Api.Application.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Squad.Api.Application.Response
{
    public class GetSquadResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Card> Lineup { get; set; }
        public Card[] Subs { get; set; }
    }
}
