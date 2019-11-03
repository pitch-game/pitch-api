using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class Foul : EventBase
    {
        public Foul(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Foul";
        public override bool ShowInTimeline => false;
    }
}
