using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
{
    public class MatchMinuteBuilder
    {
        private readonly IList<IEvent> _events = new List<IEvent>();
        private readonly MinuteStats _stats;
        private readonly IList<Modifier> _modifiers = new List<Modifier>();

        public MatchMinuteBuilder(Guid homeSquadId)
        {
            _stats = new MinuteStats(homeSquadId, 50, 50);
        }

        public MatchMinuteBuilder WithEvent(IEvent @event)
        {
            _events.Add(@event);
            return this;
        }

        public MatchMinute Build()
        {
            return new MatchMinute
            {
                Events =  _events,
                Stats = _stats,
                Modifiers = _modifiers
            };
        }
    }
}
