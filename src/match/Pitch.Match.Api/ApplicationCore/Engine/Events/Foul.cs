using System;

namespace Pitch.Match.Api.ApplicationCore.Engine.Events
{
    public class Foul : EventBase, IEvent
    {
        public Foul(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Foul";

        public bool ShowInTimeline => false;
    }
}
