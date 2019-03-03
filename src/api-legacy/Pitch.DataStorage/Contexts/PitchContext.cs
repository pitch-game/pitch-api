using Microsoft.EntityFrameworkCore;
using Pitch.DataStorage.Models;

namespace Pitch.DataStorage.Contexts
{
    public class PitchContext : DbContext
    {
        public PitchContext(DbContextOptions<PitchContext> options) : base(options) {}

        public DbSet<Card> Cards { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
    }
}
