using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Repositories
{
    public interface ICardRepository
    {
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
    }
}
