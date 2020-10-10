using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Builders
{
    public class SquadBuilder
    {
        private Guid _id;
        private readonly Dictionary<string, IEnumerable<Card>> _lineup = new Dictionary<string, IEnumerable<Card>>();
        private Card[] _subs = new Card[0];

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

        public SquadBuilder WithSubs(Card[] subs)
        {
            _subs = subs;
            return this;
        }

        public Squad Build()
        {
            return new Squad()
            {
                Id = _id,
                Lineup = _lineup,
                Subs = _subs
            };
        }
    }
}
