using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public interface IEvent
    {
        Guid CardId { get; set; }
        Guid SquadId { get; set; }
        string Name { get; }
        bool ShowInTimeline { get; }
    }
}
