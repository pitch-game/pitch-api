using Microsoft.EntityFrameworkCore;

namespace Pitch.Card.Api.Infrastructure
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> options) : base(options) { }

        public DbSet<Models.Card> Cards { get; set; }
    }
}