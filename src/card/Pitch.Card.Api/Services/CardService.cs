using AutoMapper;
using EasyNetQ;
using Pitch.Card.Api.Application.Requests;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure.Repositories;
using Pitch.Card.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Services
{
    public class CardService : ICardService
    {
        private readonly IBus _bus;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        public CardService(IBus bus, ICardRepository cardRepository, IMapper mapper)
        {
            _bus = bus;
            _cardRepository = cardRepository;
            _mapper = mapper;
        }
        public async Task<Models.Card> GetAsync(Guid id)
        {
            return await _cardRepository.GetAsync(id);
        }
        public async Task<Models.Card> CreateCardAsync(CreateCardModel createCardReq)
        {
            var request = _mapper.Map<PlayerRequest>(createCardReq);
            var player = await _bus.RequestAsync<PlayerRequest, PlayerResponse>(request);
            var card = new Models.Card()
            {
                Id = Guid.NewGuid(),
                PlayerId = player.Id,
                UserId = createCardReq.UserId,
                Name = player.Name,
                ShortName = player.ShortName,
                Position = player.Positions[0], //todo randomise,
                Rating = player.Rating,
                Rarity = CardRarity(player.Rating),
                Form = player.Form,
                Opened = true,
                Fitness = 100,
                CreatedOn = DateTime.Now
            };
            await _cardRepository.AddAsync(card);
            return card;
        }

        public async Task<IEnumerable<Models.Card>> GetAllAsync(CardRequestModel req, string userId)
        {
            return await _cardRepository.GetAllAsync(req, userId);
        }

        private string CardRarity(int rating)
        {
            switch (rating)
            {
                case int n when (n >= 80):
                    return "gold";
                case int n when (n >= 70 && n < 80):
                    return "silver";
                case int n when (n < 70):
                    return "bronze";
            }
            throw new Exception();
        }
    }
}
