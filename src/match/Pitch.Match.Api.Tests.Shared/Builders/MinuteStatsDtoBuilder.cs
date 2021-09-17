using System;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class MinuteStatsDtoBuilder
    {
        private Guid _squadInPossession;

        public MinuteStatsDtoBuilder WithSquadInPossession(Guid squadId)
        {
            _squadInPossession = squadId;
            return this;
        }

        public Infrastructure.Models.MinuteStats Build()
        {
            return new Infrastructure.Models.MinuteStats()
            {
                SquadIdInPossession = _squadInPossession,
                HomePossessionChance = 50,
                AwayPossessionChance = 50
            };
        }
    }
}
