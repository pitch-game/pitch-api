using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.Infrastructure.Repositories
{
    public interface IMatchRepository
    {
        Task<ApplicationCore.Models.Match.Match> CreateAsync(ApplicationCore.Models.Match.Match match);
        Task<ApplicationCore.Models.Match.Match> GetAsync(Guid id);
        Task<ApplicationCore.Models.Match.Match> UpdateAsync(ApplicationCore.Models.Match.Match match);
        Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetUnclaimedAsync(Guid userId);
        Task<bool> HasUnclaimedAsync(Guid userId);
        Task<Guid?> GetInProgressAsync(Guid userId);
        Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetAllAsync(int skip, int take, Guid userId);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly IDataContext<ApplicationCore.Models.Match.Match> _dataContext;

        public MatchRepository(IDataContext<ApplicationCore.Models.Match.Match> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ApplicationCore.Models.Match.Match> GetAsync(Guid id)
        {
            return await _dataContext.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid?> GetInProgressAsync(Guid userId)
        {
            var minStartDate = DateTime.Now.AddMinutes(-90);
            var result = await _dataContext.FirstOrDefaultAsync(x => x.KickOff > minStartDate && (x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId));
            return result?.Id;
        }

        public async Task<ApplicationCore.Models.Match.Match> CreateAsync(ApplicationCore.Models.Match.Match match)
        {
            await _dataContext.CreateAsync(match);
            return match;
        }

        public async Task<ApplicationCore.Models.Match.Match> UpdateAsync(ApplicationCore.Models.Match.Match match)
        {
            await _dataContext.UpdateAsync(match);
            return match;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetUnclaimedAsync(Guid userId)
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

        public async Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetAllAsync(int skip, int take, Guid userId)
        {
            //TODO include orderby and skip/take in query
            var result = await _dataContext.ToListAsync(x => x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId);
            return result.OrderByDescending(x => x.KickOff).Skip(skip).Take(take);
        }
    }
}