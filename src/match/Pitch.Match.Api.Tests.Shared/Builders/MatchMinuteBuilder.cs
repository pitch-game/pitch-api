using System.Collections.Generic;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class MatchMinuteDtoBuilder
    {
        private readonly IList<Infrastructure.Models.Event> _events = new List<Infrastructure.Models.Event>();
        private MinuteStatsDtoBuilder _stats = new MinuteStatsDtoBuilder();
        private readonly IList<Infrastructure.Models.Modifier> _modifiers = new List<Infrastructure.Models.Modifier>();

        public MatchMinuteDtoBuilder WithEvent(Infrastructure.Models.Event @event)
        {
            _events.Add(@event);
            return this;
        }

        public MatchMinuteDtoBuilder WithMinuteStats(MinuteStatsDtoBuilder minuteStats)
        {
            _stats = minuteStats;
            return this;
        }

        public Infrastructure.Models.MatchMinute Build()
        {
            return new Infrastructure.Models.MatchMinute
            {
                Events =  _events,
                Stats = _stats.Build(),
                Modifiers = _modifiers
            };
        }
    }
}
