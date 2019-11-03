using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class ShotOffTarget : EventBase
    {
        public ShotOffTarget(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Shot Off Target";
        public override bool ShowInTimeline => true;
    }
}
