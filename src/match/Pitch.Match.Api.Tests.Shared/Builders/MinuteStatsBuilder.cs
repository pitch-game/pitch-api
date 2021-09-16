using System;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class MinuteStatsBuilder
    {
        private Guid _squadInPossession;

        public MinuteStatsBuilder WithSquadInPossession(Guid squadId)
        {
            _squadInPossession = squadId;
            return this;
        }

        public MinuteStats Build()
        {
            return new MinuteStats(_squadInPossession, 50, 50);
        }
    }
}
