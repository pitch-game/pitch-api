using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Builders
{
    public class SquadBuilder
    {
        private Guid _id;
        private readonly Dictionary<string, IEnumerable<Card>> _lineup = new Dictionary<string, IEnumerable<Card>>();

        public SquadBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SquadBuilder WithCardsInLineup(string position, IEnumerable<Card> cards)
        {
            _lineup.Add(position, cards);
            return this;
        }

        public Squad Build()
        {
            return new Squad()
            {
                Id = _id,
                Lineup = _lineup
            };
        }
    }
}
