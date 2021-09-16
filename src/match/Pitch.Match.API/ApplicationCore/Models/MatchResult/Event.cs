using System;

namespace Pitch.Match.Api.ApplicationCore.Models.MatchResult
{
    public class Event
    {
        public int Minute { get; set; }
        public string Name { get; set; }
        public string SquadName { get; set; }
        public Guid CardId { get; set; }
    }
}