using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Infrastructure.Repositories
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
        private readonly IMongoCollection<ApplicationCore.Models.Match.Match> _matches;

        public MatchRepository(IConfiguration config, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("match");
            _matches = database.GetCollection<ApplicationCore.Models.Match.Match>("matches");
        }

        public async Task<ApplicationCore.Models.Match.Match> GetAsync(Guid id)
        {
            return await _matches.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Guid?> GetInProgressAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().Where(x => x.KickOff > minStartDate && (x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId)).Select(x => (Guid?)x.Id).FirstOrDefaultAsync();
        }

        public async Task<ApplicationCore.Models.Match.Match> CreateAsync(ApplicationCore.Models.Match.Match match)
        {
            await _matches.InsertOneAsync(match);
            return match;
        }

        public async Task<ApplicationCore.Models.Match.Match> UpdateAsync(ApplicationCore.Models.Match.Match match)
        {
            await _matches.ReplaceOneAsync(x => x.Id == match.Id, match);
            return match;
        }

        public async Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetUnclaimedAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().Where(x => x.KickOff <= minStartDate && ((x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards)
            || (x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards))).ToListAsync();
        }

        public async Task<bool> HasUnclaimedAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().AnyAsync(x => x.KickOff <= minStartDate && ((x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards)
            || (x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards)));
        }

        public async Task<IEnumerable<ApplicationCore.Models.Match.Match>> GetAllAsync(int skip, int take, Guid userId)
        {
            var query = _matches.AsQueryable().Where(x => x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId).OrderByDescending(x => x.KickOff).Skip(skip).Take(take);
            var results = await query.ToListAsync();
            return results;
        }
    }
}
