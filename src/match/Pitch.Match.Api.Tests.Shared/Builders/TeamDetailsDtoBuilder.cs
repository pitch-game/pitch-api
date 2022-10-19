using System;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class TeamDetailsDtoBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private SquadDtoBuilder _squad = new SquadDtoBuilder();
        private bool _hasClaimedRewards;
        private int _usedSubs;

        public TeamDetailsDtoBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public TeamDetailsDtoBuilder WithSquad(SquadDtoBuilder squad)
        {
            _squad = squad;
            return this;
        }

        public TeamDetailsDtoBuilder WithHasClaimedRewards(bool hasClaimedRewards)
        {
            _hasClaimedRewards = hasClaimedRewards;
            return this;
        }

        public TeamDetailsDtoBuilder WithUsedSubs(int usedSubs)
        {
            _usedSubs = usedSubs;
            return this;
        }

        public Infrastructure.Models.TeamDetails Build()
        {
            return new Infrastructure.Models.TeamDetails()
            {
                UserId = _userId,
                Squad = _squad.Build(),
                HasClaimedRewards = _hasClaimedRewards,
                UsedSubs = _usedSubs
            };
        }
    }
}
