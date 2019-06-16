using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Infrastructure.Repositories
{
    public interface IMatchRepository
    {
        Task<Models.Match> CreateAsync(Models.Match match);
        Task<Models.Match> GetAsync(Guid id);
        Task<Models.Match> UpdateAsync(Models.Match match);
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
    }
}
