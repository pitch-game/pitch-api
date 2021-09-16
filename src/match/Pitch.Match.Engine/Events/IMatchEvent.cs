using System;

namespace Pitch.Match.Engine.Events
{
    public interface IEvent
    {
        Guid CardId { get; set; }
        Guid SquadId { get; set; }
        string Name { get; }
        bool ShowInTimeline { get; }
    }
}
