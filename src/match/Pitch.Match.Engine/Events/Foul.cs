using System;

namespace Pitch.Match.Engine.Events
{
    public sealed class Foul : EventBase
    {
        public Foul(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Foul";
        public override bool ShowInTimeline => false;
    }
}
