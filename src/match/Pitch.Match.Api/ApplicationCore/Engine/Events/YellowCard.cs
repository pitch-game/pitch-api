using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public class YellowCard : EventBase, IEvent
    {
        public YellowCard(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Yellow Card";

        public bool ShowInTimeline => true;
    }
}
