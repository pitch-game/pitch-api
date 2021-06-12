using System;
using Pitch.Match.API.ApplicationCore.Models.Match;

namespace Pitch.Match.API.Tests.Builders
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
