using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.Api.Infrastructure.Repositories
{
    public interface IMatchRepository
    {
        Task<Models.Match> CreateAsync(Models.Match match);
        Task<Models.Match> GetAsync(Guid id);
        Task<Models.Match> UpdateAsync(Models.Match match);
        Task<IEnumerable<Models.Match>> GetUnclaimedAsync(Guid userId);
        Task<bool> HasUnclaimedAsync(Guid userId);
        Task<Guid?> GetInProgressAsync(Guid userId);
        Task<IEnumerable<Models.Match>> GetAllAsync(int skip, int take, Guid userId);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly IDataContext<Models.Match> _dataContext;

        public MatchRepository(IDataContext<Models.Match> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Models.Match> GetAsync(Guid id)
        {
            return await _dataContext.FindOneAsync(x => x.Id == id);
        }

        public async Task<Guid?> GetInProgressAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90); //TODO Fix
            var result = await _dataContext.FindOneAsync(x => x.KickOff > minStartDate && (x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId));
            return result?.Id;
        }

        public async Task<Models.Match> CreateAsync(Models.Match match)
        {
            await _dataContext.CreateAsync(match);
            return match;
        }

        public async Task<Models.Match> UpdateAsync(Models.Match match)
        {
            await _dataContext.UpdateAsync(match);
            return match;
        }

        public async Task<IEnumerable<Models.Match>> GetUnclaimedAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90); //TODO Fix
            return await _dataContext.FindAsync(x =>
                x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards
                                              || x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards));
        }

        public async Task<bool> HasUnclaimedAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90); //TODO Fix
            var result = await _dataContext.FindAsync(x =>
                x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards
                                              || x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards));
            return result.Any();
        }

        public async Task<IEnumerable<Models.Match>> GetAllAsync(int skip, int take, Guid userId)
        {
            //TODO include orderby and skip/take in query
            var result = await _dataContext.FindAsync(x => x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId);
            return result.OrderByDescending(x => x.KickOff).Skip(skip).Take(take);
        }
    }
}