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
        Task<Models.Match> CreateAsync(Models.Match match);
        Task<Models.Match> GetAsync(Guid id);
        Task<Models.Match> UpdateAsync(Models.Match match);
        Task<IEnumerable<Models.Match>> GetUnclaimedAsync(Guid userId);
        Task<bool> HasUnclaimedAsync(Guid userId);
        Task<bool> GetInProgressAsync(Guid userId);
        Task<IEnumerable<Models.Match>> GetAllAsync(int skip, int take, Guid userId);
    }

    public class MatchRepository : IMatchRepository
    {
        private readonly IMongoCollection<Models.Match> _matches;

        public MatchRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("match");
            _matches = database.GetCollection<Models.Match>("matches");
        }

        public async Task<Models.Match> GetAsync(Guid id)
        {
            return await _matches.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> GetInProgressAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().AnyAsync(x => x.KickOff > minStartDate && x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId);
        }

        public async Task<Models.Match> CreateAsync(Models.Match match)
        {
            await _matches.InsertOneAsync(match);
            return match;
        }

        public async Task<Models.Match> UpdateAsync(Models.Match match)
        {
            await _matches.ReplaceOneAsync(x => x.Id == match.Id, match);
            return match;
        }

        public async Task<IEnumerable<Models.Match>> GetUnclaimedAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().Where(x => x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards)
            || (x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards)).ToListAsync();
        }

        public async Task<bool> HasUnclaimedAsync(Guid userId)
        {
            //TODO get rid of extra time?
            var minStartDate = DateTime.Now.AddMinutes(-90);
            return await _matches.AsQueryable().AnyAsync(x => x.KickOff <= minStartDate && (x.HomeTeam.UserId == userId && !x.HomeTeam.HasClaimedRewards)
            || (x.AwayTeam.UserId == userId && !x.AwayTeam.HasClaimedRewards));
        }

        public async Task<IEnumerable<Models.Match>> GetAllAsync(int skip, int take, Guid userId)
        {
            var query = _matches.AsQueryable().Where(x => x.HomeTeam.UserId == userId || x.AwayTeam.UserId == userId).OrderByDescending(x => x.KickOff).Skip(skip).Take(take);
            var results = await query.ToListAsync();
            return results;
        }
    }
}
