using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class Substitution : EventBase
    {
        public Substitution(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Substitution";
        public override bool ShowInTimeline => true;
    }
}
