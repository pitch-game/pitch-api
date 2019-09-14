using AutoMapper;
using EasyNetQ;
using Pitch.Card.API.Application.Requests;
using Pitch.Card.API.Application.Responses;
using Pitch.Card.API.Infrastructure.Repositories;
using Pitch.Card.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Card.API.Infrastructure.Services
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
        public async Task<IEnumerable<Models.Card>> GetAsync(IEnumerable<Guid> ids)
        {
            return await _cardRepository.GetAsync(ids);
        }

        public async Task<Models.Card> CreateCardAsync(CreateCardModel createCardReq)
        {
            var request = _mapper.Map<PlayerRequest>(createCardReq);
            var player = await _bus.RequestAsync<PlayerRequest, PlayerResponse>(request);

            var random = new Random();
            int index = random.Next(player.Positions.Length);
            var position = createCardReq.Position != null ? player.Positions.First(x => x == createCardReq.Position) : player.Positions[index];

            var card = new Models.Card()
            {
                Id = Guid.NewGuid(),
                PlayerId = player.Id,
                UserId = createCardReq.UserId,
                Name = player.Name,
                ShortName = player.ShortName,
                Position = position,
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
            req.PositionPriority = PositionMap(req.PositionPriority);
            return await _cardRepository.GetAllAsync(req, userId);
        }

        public async Task SetGoals(IDictionary<Guid, int> scorers)
        {
            foreach (var scorer in scorers)
            {
                var card = await _cardRepository.GetAsync(scorer.Key);
                card.GoalsScored += scorer.Value;
                await _cardRepository.UpdateAsync(card);
            }
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

        //todo frontend responsibility?
        private string PositionMap(string position)
        {
            switch (position)
            {
                case "RST":
                case "LST":
                    return "ST";
                case "RCM":
                case "LCM":
                    return "CM";
                case "RCB":
                case "LCB":
                    return "CB";
                default:
                    return position;
            }
        }
    }
}
