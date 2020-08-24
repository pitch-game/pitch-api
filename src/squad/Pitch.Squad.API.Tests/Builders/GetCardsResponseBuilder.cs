using System;
using System.Collections.Generic;
using Pitch.Squad.API.Application.Models;
using Pitch.Squad.API.Application.Response;

namespace Pitch.Squad.API.Tests.Builders
{
    public class GetCardsResponseBuilder
    {
        private readonly GetCardsResponse _getCardsResponse;

        private readonly IList<CardDTO> _cards = new List<CardDTO>();

        public GetCardsResponseBuilder()
        {
            _getCardsResponse = new GetCardsResponse()
            {
                Cards = _cards
            };
        }

        public GetCardsResponseBuilder WithDefaultCard(Guid cardId, string userId)
        {
            _cards.Add(new CardDTO()
            {
                Id = cardId,
                UserId = userId,
                Name = "Test",
                Fitness = 100,
                Position = "CM",
                ShortName = "Test",
                Rarity = "Rare"
            });

            return this;
        }

        public GetCardsResponse Build()
        {
            return _getCardsResponse;
        }
    }
}
