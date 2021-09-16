using System;
using System.Linq;
using Pitch.Match.Engine;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class MatchDtoBuilder
    {
        private Guid _id = Guid.NewGuid();
        private TeamDetailsBuilder _awayTeam = new TeamDetailsBuilder();
        private TeamDetailsBuilder _homeTeam = new TeamDetailsBuilder();
        private DateTime _kickOff = DateTime.Now; 
        private readonly MatchMinuteBuilder[] _matchMinutes = Enumerable.Range(0, Constants.MatchLengthInMinutes).Select(i => new MatchMinuteBuilder()).ToArray();
        private int _version = 0;

        public MatchDtoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public MatchDtoBuilder WithHomeTeam(TeamDetailsBuilder homeTeam)
        {
            _homeTeam = homeTeam;
            return this;
        }

        public MatchDtoBuilder WithAwayTeam(TeamDetailsBuilder awayTeam)
        {
            _awayTeam = awayTeam;
            return this;
        }

        public MatchDtoBuilder WithKickOff(DateTime kickOff)
        {
            _kickOff = kickOff;
            return this;
        }

        public MatchDtoBuilder WithMinute(int minute, MatchMinuteBuilder matchMinute)
        {
            _matchMinutes[minute] = matchMinute;
            return this;
        }

        public Infrastructure.Models.Match Build()
        {
            return new Infrastructure.Models.Match()
            {
                Id = _id,
                AwayTeam = _awayTeam.Build(),
                HomeTeam = _homeTeam.Build(),
                KickOff = _kickOff,
                Minutes = _matchMinutes.Select(x => x.Build()).ToArray(),
                Version = _version
            };
        }
    }
}
