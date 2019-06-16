using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    public interface IEvent
    {
        Guid CardId { get; set; }
        Guid SquadId { get; set; }
        int Minute { get; set; }
        string Name { get; }
    }
}
