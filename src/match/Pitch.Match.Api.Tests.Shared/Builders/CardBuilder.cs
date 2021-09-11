using System;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.Api.Tests.Shared.Builders
{
    public class CardBuilder
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

        public CardBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public CardBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Card Build()
        {
            return new Card()
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
