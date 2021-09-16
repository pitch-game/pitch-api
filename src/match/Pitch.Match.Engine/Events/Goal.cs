using System;

namespace Pitch.Match.Engine.Events
{
    public sealed class Goal : EventBase
    {
        public Goal(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Goal";
        public override bool ShowInTimeline => true;
    }
}
