using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class ShotOffTarget : EventBase
    {
        public ShotOffTarget(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Shot Off Target";
        public override bool ShowInTimeline => true;
    }
}
