using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public abstract class EventBase : IEvent
    {
        protected EventBase(int minute, Guid cardId, Guid squadId)
        {
            Minute = minute;
            CardId = cardId;
            SquadId = squadId;
        }

        public int Minute { get; set; }
        public Guid CardId { get; set; }
        public Guid SquadId { get; set; }

        public abstract string Name { get; }
        public abstract bool ShowInTimeline { get; }
    }
}
