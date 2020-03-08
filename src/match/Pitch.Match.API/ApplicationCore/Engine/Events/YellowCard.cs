using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Events
{
    public sealed class YellowCard : EventBase
    {
        public YellowCard(Guid cardId, Guid squadId) : base(cardId, squadId) { }

        public override string Name => "Yellow Card";
        public override bool ShowInTimeline => true;
    }
}
