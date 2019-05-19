using Microsoft.EntityFrameworkCore;
using Pitch.Store.Api.Models;

namespace Pitch.Store.Api.Infrastructure
{
    public class PackDBContext : DbContext
    {
        public PackDBContext(DbContextOptions<PackDBContext> options) : base(options) {}

        public DbSet<Pack> Packs { get; set; }
    }
}