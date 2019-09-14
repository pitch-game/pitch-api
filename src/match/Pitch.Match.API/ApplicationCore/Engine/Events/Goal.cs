using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public class Goal : EventBase, IEvent
    {
        public Goal(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Goal";
        public bool ShowInTimeline => true;
    }
}
