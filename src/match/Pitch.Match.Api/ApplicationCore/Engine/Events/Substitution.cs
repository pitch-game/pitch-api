using System;

namespace Pitch.Match.Api.ApplicationCore.Engine.Events
{
    public class Substitution : EventBase, IEvent
    {
        public Substitution(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Substitution";

        //TODO add subbed on card id

        public bool ShowInTimeline => true;
    }
}
