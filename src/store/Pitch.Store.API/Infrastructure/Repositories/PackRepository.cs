using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pitch.Store.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Store.API.Infrastructure.Repositories
{
    public interface IPackRepository
    {
        Task<IList<Pack>> GetAllAsync(string userId);
        Task<Pack> GetAsync(Guid id);
        Task<EntityEntry<Pack>> AddAsync(Pack pack);
        Task<int> SaveChangesAsync();
        Task Delete(Guid id);
    }

    public class PackRepository : IPackRepository
    {
        PackDBContext _context;
        public PackRepository(PackDBContext context)
        {
            _context = context;
        }

        public async Task<IList<Pack>> GetAllAsync(string userId)
        {
            return await _context.Packs.Where(x => x.UserId == userId).ToListAsync();
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

        public async Task Delete(Guid id)
        {
            var entity = await _context.Packs.FindAsync(id);
            if(entity != null)
                _context.Remove(entity);
        }
    }
}
