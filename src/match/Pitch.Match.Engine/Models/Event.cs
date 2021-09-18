using System;

namespace Pitch.Match.Engine.Models
{
    public class Event
    {
        public Event(EventType type, Guid cardId, Guid squadId)
        {
            Type = type;
            CardId = cardId;
            SquadId = squadId;
        }

        public Event()
        {
            
        }

        public Guid CardId { get; set; }
        public Guid SquadId { get; set; }
        public EventType Type { get; set; }
    }

    public enum EventType
    {
        Foul,
        YellowCard,
        RedCard,
        ShotOffTarget,
        ShotOnTarget,
        Goal,
        Sub
    }
}
