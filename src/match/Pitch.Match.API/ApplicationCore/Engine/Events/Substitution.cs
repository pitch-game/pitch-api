using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class Substitution : EventBase
    {
        public Substitution(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Substitution";
        public override bool ShowInTimeline => true;
    }
}
