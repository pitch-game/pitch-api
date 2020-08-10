using System;
using System.Threading.Tasks;

namespace Pitch.Squad.API.Infrastructure.Repositories
{
    public interface ISquadRepository
    {
        Task<Models.Squad> GetAsync(string userId);
        Task<Models.Squad> CreateAsync(string userId);
        Task<Models.Squad> UpdateAsync(Models.Squad squad);
    }

    public class SquadRepository : ISquadRepository
    {
        private readonly IDataContext<Models.Squad> _squadContext;

        public SquadRepository(IDataContext<Models.Squad> squadContext)
        {
            _squadContext = squadContext;
        }

        public async Task<Models.Squad> GetAsync(string userId)
        {
            return await _squadContext.FindOneAsync(x => x.UserId == userId);
        }

        public async Task<Models.Squad> CreateAsync(string userId)
        {
            var squad = new Models.Squad() { UserId = userId, Id = Guid.NewGuid() };
            await _squadContext.CreateAsync(squad);
            return squad;
        }

        public async Task<Models.Squad> UpdateAsync(Models.Squad squad)
        {
            squad.LastUpdated = DateTime.Now;
            await _squadContext.UpdateAsync(squad);
            return squad;
        }
    }
}
