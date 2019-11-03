using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class RedCard : EventBase
    {
        public RedCard(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Red Card";

        public override bool ShowInTimeline => true;
    }
}
