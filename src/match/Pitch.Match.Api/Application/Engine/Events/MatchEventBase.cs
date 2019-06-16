using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    public class EventBase
    {
        public EventBase(int minute, Guid cardId, Guid squadId)
        {
            Minute = minute;
            CardId = cardId;
            SquadId = squadId;
        }

        public int Minute { get; set; }
        public Guid CardId { get; set; }
        public Guid SquadId { get; set; }
    }
}
