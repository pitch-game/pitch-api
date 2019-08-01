using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.MessageBus.Responses
{
    public class GetSquadResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Card> Lineup { get; set; }
        public Card[] Subs { get; set; }
    }
}
