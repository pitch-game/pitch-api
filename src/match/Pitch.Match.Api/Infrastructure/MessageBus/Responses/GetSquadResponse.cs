using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Infrastructure.MessageBus.Responses
{
    public class GetSquadResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Engine.Models.Card> Lineup { get; set; } //TODO Core Model
        public Engine.Models.Card[] Subs { get; set; }
    }
}
