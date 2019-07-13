using System;

namespace Pitch.Match.Api.Models
{
    public class SubRequest
    {
        public Guid Off { get; set; }
        public Guid On { get; set; }
        public Guid MatchId { get; set; }
    }
}
