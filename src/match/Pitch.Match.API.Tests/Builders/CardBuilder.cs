using System;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Builders
{
    public class CardBuilder
    {
        private Guid _id = Guid.NewGuid();

        public CardBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public Card Build()
        {
            return new Card()
            {
                Id = _id
            };
        }
    }
}
