using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Infrastructure
{
    public class SquadDbContext : DbContext
    {
        public SquadDbContext(DbContextOptions<SquadDbContext> options) : base(options) { }

        public DbSet<Models.Squad> Squads { get; set; }
    }
}
