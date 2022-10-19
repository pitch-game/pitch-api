using System;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.Api.Infrastructure.Models
{
    public class Match : IEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }

        public virtual TeamDetails HomeTeam { get; set; }
        public virtual TeamDetails AwayTeam { get; set; }

        public DateTime KickOff { get; set; }

        public MatchMinute[] Minutes { get; set; }
    }
}
