using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    public class RedCard : EventBase, IEvent
    {
        public RedCard(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Red Card";

        public bool ShowInTimeline => true;
    }
}
