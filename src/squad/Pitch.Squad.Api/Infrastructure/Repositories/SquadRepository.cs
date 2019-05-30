using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Infrastructure.Repositories
{
    public class SquadRepository : ISquadRepository
    {
        private readonly IMongoCollection<Models.Squad> _squads;

        public SquadRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("squad");
            _squads = database.GetCollection<Models.Squad>("squads");
        }

        public async Task<Models.Squad> GetAsync(string userId)
        {
            return await _squads.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Models.Squad> CreateAsync(string userId)
        {
            var squad = new Models.Squad() { UserId = userId, Id = Guid.NewGuid() };
            await _squads.InsertOneAsync(squad);
            return squad;
        }

        public async Task<Models.Squad> UpdateAsync(Models.Squad squad)
        {
            await _squads.ReplaceOneAsync(x => x.Id == squad.Id, squad);
            return squad;
        }
    }
}
