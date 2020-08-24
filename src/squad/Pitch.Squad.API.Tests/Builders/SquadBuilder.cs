using System;
using System.Collections.Generic;

namespace Pitch.Squad.API.Tests.Builders
{
    public class SquadBuilder
    {
        private readonly Models.Squad _squad;

        private readonly Guid _defaultSquadId = Guid.NewGuid();
        private readonly string _defaultUserId = Guid.NewGuid().ToString();

        public readonly Guid DefaultLstId = Guid.NewGuid();
        public readonly Guid DefaultRstId = Guid.NewGuid();

        public readonly Guid DefaultLmId = Guid.NewGuid();
        public readonly Guid DefaultLcmId = Guid.NewGuid();
        public readonly Guid DefaultRcmId = Guid.NewGuid();
        public readonly Guid DefaultRmId = Guid.NewGuid();

        public readonly Guid DefaultLbId = Guid.NewGuid();
        public readonly Guid DefaultLcbId = Guid.NewGuid();
        public readonly Guid DefaultRcbId = Guid.NewGuid();
        public readonly Guid DefaultRbId = Guid.NewGuid();

        public readonly Guid DefaultGkId = Guid.NewGuid();
        
        public SquadBuilder()
        {
            _squad = new Models.Squad();
        }

        public SquadBuilder WithDefaults()
        {
            _squad.Id = _defaultSquadId;
            _squad.UserId = _defaultUserId;
            _squad.Lineup = new Dictionary<string, Guid?>()
            {
                {"LST", DefaultLstId},
                {"RST", DefaultRstId},
                {"LM", DefaultLmId},
                {"LCM", DefaultLcmId},
                {"RCM", DefaultRcmId},
                {"RM", DefaultRmId},
                {"LB", DefaultLbId},
                {"LCB", DefaultLcbId},
                {"RCB", DefaultRcbId},
                {"RB", DefaultRbId},
                {"GK", DefaultGkId}
            };

            return this;
        }

        public SquadBuilder SetPosition(string position, Guid guid)
        {
            _squad.Lineup[position] = guid;
            return this;
        }

        public SquadBuilder WithId(Guid id)
        {
            _squad.Id = id;
            return this;
        }

        public SquadBuilder WithLineup(Dictionary<string, Guid?> lineup)
        {
            _squad.Lineup = lineup;
            return this;
        }

        public Models.Squad Build()
        {
            return _squad;
        }
    }
}
