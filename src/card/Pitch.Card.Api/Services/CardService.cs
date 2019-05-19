using EasyNetQ;
using Pitch.Card.Api.Application.Requests;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure.Repositories;
using Pitch.Card.Api.Models;
using System;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Services
{
    public class CardService : ICardService
    {
        private readonly IBus _bus;
        private readonly ICardRepository _cardRepository;
        public CardService(IBus bus, ICardRepository cardRepository)
        {
            _bus = bus;
            _cardRepository = cardRepository;
        }
        public async Task<Models.Card> GetAsync(Guid id)
        {
            return await _cardRepository.GetAsync(id);
        }
        public async Task<Models.Card> CreateCardAsync()
        {
            var request = new PlayerRequest((55, 99));
            var player = await _bus.RequestAsync<PlayerRequest, PlayerResponse>(request);
            var card = new Models.Card()
            {
                Id = Guid.NewGuid(),
                PlayerId = player.Id,
                Name = player.Name,
                Position = player.Positions[0], //todo randomise,
                Rating = player.Rating,
                Rarity = "TODO",
                Form = player.Form
            };
            await _cardRepository.AddAsync(card);
            return card;
        }
    }
}
