using System;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
{
    public class MatchBuilder
    {
        private Guid _id = Guid.NewGuid();
        private TeamDetails _awayTeam = new TeamDetailsBuilder().Build();
        private TeamDetails _homeTeam = new TeamDetailsBuilder().Build();
        private DateTime _kickOff = DateTime.Now; 
        private MatchMinute[] _minutes = new MatchMinute[0]; //TODO builder

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

        public MatchBuilder WithMinutes(MatchMinute[] minutes)
        {
            _minutes = minutes;
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
                //Minutes = _minutes
            };
        }
    }
}
