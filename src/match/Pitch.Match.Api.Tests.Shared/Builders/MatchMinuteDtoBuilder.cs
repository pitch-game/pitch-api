﻿using System.Collections.Generic;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class MatchMinuteBuilder
    {
        private readonly IList<Event> _events = new List<Event>();
        private MinuteStatsBuilder _stats = new MinuteStatsBuilder();
        private readonly IList<Modifier> _modifiers = new List<Modifier>();

        public MatchMinuteBuilder WithEvent(Event @event)
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
