using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pitch.Card.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Models.Card>> GetAllAsync(CardRequestModel req, string userId);
        Task<IEnumerable<Models.Card>> GetAsync(IEnumerable<Guid> ids);
        Task<Models.Card> AddAsync(Models.Card card);
    }

    public class CardRepository : ICardRepository
    {
        private readonly IMongoCollection<Models.Card> _cards;

        public CardRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("card");
            _cards = database.GetCollection<Models.Card>("cards");
        }

        public async Task<IEnumerable<Models.Card>> GetAsync(IEnumerable<Guid> ids)
        {
            return await _cards.Find(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Models.Card> AddAsync(Models.Card card)
        {
            await _cards.InsertOneAsync(card);
            return card;
        }

        public async Task<IEnumerable<Models.Card>> GetAllAsync(CardRequestModel req, string userId)
        {
            var query = _cards.AsQueryable().Where(x => x.UserId == userId);
            if (req.NotIn != null && req.NotIn.Any())
            {
                query = query.Where(x => !req.NotIn.Contains(x.Id));
            }
            var results = await query.ToListAsync();
            return results.OrderByDescending(x => x.Position == req.PositionPriority).ThenByDescending(x => x.Rating).Skip(req.Skip).Take(req.Take);
        }
    }
}
