using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pitch.User.API.Models;
using System;
using System.Threading.Tasks;


namespace Pitch.User.API.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Models.User> GetAsync(Guid id);
        Task<Models.User> GetAsync(string email);
        Task<Models.User> CreateAsync(string email);
        Task UpdateAsync(Models.User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Models.User> _users;

        public UserRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("user");
            _users = database.GetCollection<Models.User>("users");
        }

        public async Task<Models.User> GetAsync(Guid id)
        {
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Models.User> GetAsync(string email)
        {
            return await _users.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Models.User> CreateAsync(string email)
        {
            await _users.InsertOneAsync(new Models.User {
                Id = Guid.NewGuid(),
                Email = email
            });
            return await GetAsync(email); //TODO race condition?
        }

        public async Task UpdateAsync(Models.User user)
        {
            await _users.ReplaceOneAsync(x => x.Id == user.Id, user);
        }
    }
}
