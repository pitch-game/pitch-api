using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.Infrastructure.Repositories
{
    public interface IMatchRepository
    {
        Task<ApplicationCore.Models.Match> CreateAsync(ApplicationCore.Models.Match match);
        Task<ApplicationCore.Models.Match> GetAsync(Guid id);
        Task<ApplicationCore.Models.Match> UpdateAsync(ApplicationCore.Models.Match match);
        Task<IEnumerable<ApplicationCore.Models.Match>> GetUnclaimedAsync(Guid userId);
        Task<bool> HasUnclaimedAsync(Guid userId);
        Task<Guid?> GetInProgressAsync(Guid userId);
        Task<IEnumerable<ApplicationCore.Models.Match>> GetAllAsync(int skip, int take, Guid userId);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly IDataContext<ApplicationCore.Models.Match> _dataContext;

        public MatchRepository(IDataContext<ApplicationCore.Models.Match> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ApplicationCore.Models.Match> GetAsync(Guid id)
        {
            return await _dataContext.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid?> GetInProgressAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90);
            var result = await _dataContext.FirstOrDefaultAsync(x => x.KickOff > minStartDate && (x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId));
            return result?.Id;
        }

        public async Task<ApplicationCore.Models.Match> CreateAsync(ApplicationCore.Models.Match match)
        {
            await _dataContext.CreateAsync(match);
            return match;
        }

        public async Task<ApplicationCore.Models.Match> UpdateAsync(ApplicationCore.Models.Match match)
        {
            await _dataContext.UpdateAsync(match);
            return match;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Match>> GetUnclaimedAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _dataContext.ToListAsync(x =>
                x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards
                                              || x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards));
        }

        public async Task<bool> HasUnclaimedAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90);
            var result = await _dataContext.ToListAsync(x =>
                x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards
                                              || x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards));
            return result.Any();
        }

        public async Task<IEnumerable<ApplicationCore.Models.Match>> GetAllAsync(int skip, int take, Guid userId)
        {
            //TODO include orderby and skip/take in query
            var result = await _dataContext.ToListAsync(x => x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId);
            return result.OrderByDescending(x => x.KickOff).Skip(skip).Take(take);
        }
    }
}