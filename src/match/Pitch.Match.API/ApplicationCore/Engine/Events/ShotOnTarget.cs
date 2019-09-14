using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public class ShotOnTarget : EventBase, IEvent
    {
        public ShotOnTarget(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Shot Saved";

        public bool ShowInTimeline => true;
    }
}
