using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class SquadBuilder
    {
        private Guid _id;
        private readonly Dictionary<string, IEnumerable<CardBuilder>> _lineup = new Dictionary<string, IEnumerable<CardBuilder>>();
        private CardBuilder[] _subs = new CardBuilder[0];
        private string _name = string.Empty;

        public SquadBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SquadBuilder WithCardsInLineup(string position, IEnumerable<CardBuilder> cards)
        {
            _lineup.Add(position, cards);
            return this;
        }

        public SquadBuilder WithSubs(CardBuilder[] subs)
        {
            _subs = subs;
            return this;
        }

        public SquadBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Squad Build()
        {
            return new Squad()
            {
                Id = _id,
                Name = _name,
                Lineup = _lineup.ToDictionary(x => x.Key, x => x.Value.Select(cb => cb.Build())),
                Subs = _subs.Select(x => x.Build()).ToArray()
            };
        }
    }
}
