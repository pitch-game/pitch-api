using System;

namespace Pitch.Match.Api.Models
{
    public interface IMatchEvent
    {
        Guid CardId { get; set; }
        Guid SquadId { get; set; }
        int Minute { get; set; }
    }
}
