using Pitch.Store.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Store.API.Infrastructure.Repositories
{
    public interface IPackRepository
    {
        Task<IEnumerable<Pack>> GetAllAsync(string userId);
        Task<Pack> GetAsync(Guid id);
        Task AddAsync(Pack pack);
        Task Delete(Guid id);
    }

    public class PackRepository : IPackRepository
    {
        private readonly IDataContext<Pack> _packContext;
        public PackRepository(IDataContext<Pack> context)
        {
            _packContext = context;
        }

        public async Task<IEnumerable<Pack>> GetAllAsync(string userId)
        {
            return await _packContext.FindAsync(x => x.UserId == userId);
        }

        public async Task<Pack> GetAsync(Guid id)
        {
            return await _packContext.FindOneAsync(x => x.Id == id);
        }

        public async Task AddAsync(Pack pack)
        {
            await _packContext.CreateAsync(pack);
        }

        public async Task Delete(Guid id)
        {
            var entity = await _packContext.FindOneAsync(x => x.Id == id);
            if(entity != null)
                await _packContext.DeleteAsync(entity);
        }
    }
}
