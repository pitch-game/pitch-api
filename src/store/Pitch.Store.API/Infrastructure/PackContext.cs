using Microsoft.EntityFrameworkCore;
using Pitch.Store.API.Models;

namespace Pitch.Store.API.Infrastructure
{
    public class PackDBContext : DbContext
    {
        public PackDBContext(DbContextOptions<PackDBContext> options) : base(options) {}

        public DbSet<Pack> Packs { get; set; }
    }
}