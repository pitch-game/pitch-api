using System;

namespace Pitch.Match.Engine.Events
{
    public abstract class EventBase : IEvent
    {
        protected EventBase(Guid cardId, Guid squadId)
        {
            CardId = cardId;
            SquadId = squadId;
        }

        public Guid CardId { get; set; }
        public Guid SquadId { get; set; }

        public abstract string Name { get; }
        public abstract bool ShowInTimeline { get; }
    }
}
