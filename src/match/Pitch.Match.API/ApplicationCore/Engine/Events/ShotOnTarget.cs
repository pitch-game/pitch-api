using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class ShotOnTarget : EventBase
    {
        public ShotOnTarget(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Shot Saved";
        public override bool ShowInTimeline => true;
    }
}
