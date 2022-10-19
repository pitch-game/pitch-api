using System;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class CardDtoBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name;
        private readonly string _shortName = "Player Name";
        private int _rating = 50;
        private readonly string _rarity = "rare";
        private readonly string _position = "ST";
        private readonly int _chemistry = 100;
        private readonly int _fitness = 100;
        private readonly int _goals = 0;
        private readonly int _yellowCards = 0;
        private readonly int _redCards = 0;

        public CardDtoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public CardDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Infrastructure.Models.Card Build()
        {
            return new Infrastructure.Models.Card()
            {
                Id = _id,
                Name = _name,
                ShortName = _shortName,
                Rating = _rating,
                Rarity = _rarity,
                Position = _position,
                Chemistry = _chemistry,
                Fitness = _fitness,
                Goals = _goals,
                YellowCards =_yellowCards,
                RedCards = _redCards
            };
        }
    }
}
