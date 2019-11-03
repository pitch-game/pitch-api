using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class YellowCard : EventBase
    {
        public YellowCard(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public override string Name => "Yellow Card";
        public override bool ShowInTimeline => true;
    }
}
