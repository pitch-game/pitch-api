using Pitch.Match.Api.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.Infrastructure.MessageBus.Responses
{
    public class GetSquadResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Card> Lineup { get; set; }
        public Card[] Subs { get; set; }
    }
}
