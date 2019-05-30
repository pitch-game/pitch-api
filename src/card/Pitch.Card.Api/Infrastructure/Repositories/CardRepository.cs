using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pitch.Card.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Models.Card>> GetAllAsync(string userId);
        Task<Models.Card> GetAsync(Guid id);
        Task<EntityEntry<Models.Card>> AddAsync(Models.Card card);
    }

    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _cardDbContext;

        public CardRepository(CardDbContext cardDbContext)
        {
            _cardDbContext = cardDbContext;
        }

        public async Task<Models.Card> GetAsync(Guid id)
        {
            return await _cardDbContext.Cards.FindAsync(id);
        }

        public async Task<EntityEntry<Models.Card>> AddAsync(Models.Card card)
        {
            var entry = await _cardDbContext.Cards.AddAsync(card);
            await _cardDbContext.SaveChangesAsync();
            return entry;
        }

        public async Task<IEnumerable<Models.Card>> GetAllAsync(string userId)
        {
            //TODO paginate
            return await _cardDbContext.Cards.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
