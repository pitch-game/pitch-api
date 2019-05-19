using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pitch.Store.Api.Models;
using System;
using System.Threading.Tasks;

namespace Pitch.Store.Api.Infrastructure.Repositories
{
    public interface IPackRepository
    {
        Task<Pack> GetAsync(Guid id);
        Task<EntityEntry<Pack>> AddAsync(Pack pack);
        Task<int> SaveChangesAsync();
    }

    public class PackRepository : IPackRepository
    {
        PackDBContext _context;
        public PackRepository(PackDBContext context)
        {
            _context = context;
        }

        public async Task<Pack> GetAsync(Guid id)
        {
            return await _context.Packs.FindAsync(id);
        }

        public async Task<EntityEntry<Pack>> AddAsync(Pack pack)
        {
            return await _context.Packs.AddAsync(pack);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
