using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class ShotOnTarget : EventBase
    {
        public ShotOnTarget(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Shot Saved";
        public override bool ShowInTimeline => true;
    }
}
