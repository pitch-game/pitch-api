using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class SquadDtoBuilder
    {
        private Guid _id;
        private readonly Dictionary<string, IEnumerable<CardDtoBuilder>> _lineup = new Dictionary<string, IEnumerable<CardDtoBuilder>>();
        private CardDtoBuilder[] _subs = new CardDtoBuilder[0];
        private string _name = string.Empty;

        public SquadDtoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public SquadDtoBuilder WithCardsInLineup(string position, IEnumerable<CardDtoBuilder> cards)
        {
            _lineup.Add(position, cards);
            return this;
        }

        public SquadDtoBuilder WithSubs(CardDtoBuilder[] subs)
        {
            _subs = subs;
            return this;
        }

        public SquadDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Infrastructure.Models.Squad Build()
        {
            return new Infrastructure.Models.Squad()
            {
                Id = _id,
                Name = _name,
                Lineup = _lineup.ToDictionary(x => x.Key, x => x.Value.Select(cb => cb.Build())),
                Subs = _subs.Select(x => x.Build()).ToArray()
            };
        }
    }
}
