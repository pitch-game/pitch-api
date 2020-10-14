using System;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
{
    public class MatchBuilder
    {
        private Guid _id = Guid.NewGuid();
        private TeamDetails _awayTeam = new TeamDetailsBuilder().Build();
        private TeamDetails _homeTeam = new TeamDetailsBuilder().Build();
        private DateTime _kickOff = DateTime.Now; 
        private readonly MatchMinute[] _matchMinutes = Enumerable.Range(0, Constants.MatchLengthInMinutes).Select(i => new MatchMinute()).ToArray();
        private int _version = 0;

        public MatchBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public MatchBuilder WithHomeTeam(TeamDetails homeTeam)
        {
            _homeTeam = homeTeam;
            return this;
        }

        public MatchBuilder WithAwayTeam(TeamDetails awayTeam)
        {
            _awayTeam = awayTeam;
            return this;
        }

        public MatchBuilder WithKickOff(DateTime kickOff)
        {
            _kickOff = kickOff;
            return this;
        }

        public MatchBuilder WithMinute(int minute, MatchMinute matchMinute)
        {
            _matchMinutes[minute] = matchMinute;
            return this;
        }

        public ApplicationCore.Models.Match.Match Build()
        {
            return new ApplicationCore.Models.Match.Match()
            {
                Id = _id,
                AwayTeam = _awayTeam,
                HomeTeam = _homeTeam,
                KickOff = _kickOff,
                Minutes = _matchMinutes,
                Version = _version
            };
        }
    }
}
