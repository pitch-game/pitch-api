using System;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
{
    public class TeamDetailsBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private Squad _squad = new SquadBuilder().Build();
        private bool _hasClaimedRewards;
        private int _usedSubs;

        public TeamDetailsBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public TeamDetailsBuilder WithSquad(Squad squad)
        {
            _squad = squad;
            return this;
        }

        public TeamDetailsBuilder WithHasClaimedRewards(bool hasClaimedRewards)
        {
            _hasClaimedRewards = hasClaimedRewards;
            return this;
        }

        public TeamDetailsBuilder WithUsedSubs(int usedSubs)
        {
            _usedSubs = usedSubs;
            return this;
        }

        public TeamDetails Build()
        {
            return new TeamDetails()
            {
                UserId = _userId,
                Squad = _squad,
                HasClaimedRewards = _hasClaimedRewards,
                UsedSubs = _usedSubs
            };
        }
    }
}
