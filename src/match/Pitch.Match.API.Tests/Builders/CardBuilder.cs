using System;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Builders
{
    public class CardBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name;
        private string _shortName;
        private int _rating = 50;
        private string _rarity;
        private string _position;
        private int _chemistry;
        private int _fitness;
        private int _goals;
        private int _yellowCards;
        private int _redCards;

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
