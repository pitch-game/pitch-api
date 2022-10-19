using System;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class TeamDetailsBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private SquadBuilder _squad = new SquadBuilder();
        private bool _hasClaimedRewards;
        private int _usedSubs;

        public TeamDetailsBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public TeamDetailsBuilder WithSquad(SquadBuilder squad)
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
                Squad = _squad.Build(),
                HasClaimedRewards = _hasClaimedRewards,
                UsedSubs = _usedSubs
            };
        }
    }
}
