using Pitch.Match.Api.Models;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    public class Goal : IMatchEvent
    {
        public Guid CardId { get; set; }
        public Guid SquadId { get; set; }
        public int Minute { get; set; }
    }
}
