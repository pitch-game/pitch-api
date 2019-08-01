using System;

namespace Pitch.Match.Api.ApplicationCore.Engine.Events
{
    public interface IEvent
    {
        Guid CardId { get; set; }
        Guid SquadId { get; set; }
        int Minute { get; set; }
        string Name { get; }
        bool ShowInTimeline { get; }
    }
}
