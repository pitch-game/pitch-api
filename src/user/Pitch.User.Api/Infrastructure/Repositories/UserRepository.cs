using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;


namespace Pitch.User.Api.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Models.User> Get(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Models.User> _users;

        public UserRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("card");
            _users = database.GetCollection<Models.User>("cards");
        }

        public async Task<Models.User> Get(Guid id)
        {
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
