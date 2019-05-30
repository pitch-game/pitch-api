using Microsoft.EntityFrameworkCore;
using Pitch.Squad.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Infrastructure.Repositories
{
    public class SquadRepository : ISquadRepository
    {
        private readonly SquadDbContext _context;
        public SquadRepository(SquadDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Squad> GetAsync(string userId)
        {
            return await _context.Squads.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Models.Squad> CreateAsync(string userId)
        {
            var squad = new Models.Squad() { UserId = userId, Id = Guid.NewGuid() };
            await _context.SaveChangesAsync();
            return squad;
        }

        public async Task<Models.Squad> UpdateAsync(Models.Squad squad)
        {
            _context.Update(squad);
            await _context.SaveChangesAsync();
            return squad;
        }
    }
}
