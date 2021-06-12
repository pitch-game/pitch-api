using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
{
    public class MatchMinuteBuilder
    {
        private readonly IList<IEvent> _events = new List<IEvent>();
        private MinuteStatsBuilder _stats = new MinuteStatsBuilder();
        private readonly IList<Modifier> _modifiers = new List<Modifier>();

        public MatchMinuteBuilder WithEvent(IEvent @event)
        {
            _events.Add(@event);
            return this;
        }

        public MatchMinuteBuilder WithMinuteStats(MinuteStatsBuilder minuteStats)
        {
            _stats = minuteStats;
            return this;
        }

        public MatchMinute Build()
        {
            return new MatchMinute
            {
                Events =  _events,
                Stats = _stats.Build(),
                Modifiers = _modifiers
            };
        }
    }
}
