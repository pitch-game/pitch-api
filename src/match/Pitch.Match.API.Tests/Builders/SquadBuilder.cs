using System;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Builders
{
    public class SquadBuilder
    {
        private Guid _id;

        public SquadBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public Squad Build()
        {
            return new Squad()
            {
                Id = _id
            };
        }
    }
}
