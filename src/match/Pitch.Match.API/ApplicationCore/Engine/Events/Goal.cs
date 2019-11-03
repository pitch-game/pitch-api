using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class Goal : EventBase
    {
        public Goal(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Goal";
        public override bool ShowInTimeline => true;
    }
}
