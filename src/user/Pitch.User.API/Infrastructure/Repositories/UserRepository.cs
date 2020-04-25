using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using MongoDB.Driver;
using Pitch.User.API.Infrastructure.Repositories.Contexts;


namespace Pitch.User.API.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Models.User> GetAsync(Guid id);
        Task<Models.User> GetAsync(string email);
        Task<Models.User> CreateAsync(string email);
        Task UpdateAsync(Models.User user);
        Task<bool> TakePayment(Guid id, int amount);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDataContext<Models.User> _dataContext;

        public UserRepository(IDataContext<Models.User> dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Models.User> GetAsync(Guid id)
        {
            return await _dataContext.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Models.User> GetAsync(string email)
        {
            return await _dataContext.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Models.User> CreateAsync(string email)
        {
            await _dataContext.CreateAsync(new Models.User {
                Id = Guid.NewGuid(),
                Email = email
            });
            return await GetAsync(email); //TODO race condition?
        }

        public async Task UpdateAsync(Models.User user)
        {
            await _dataContext.UpdateAsync(user);
        }

        public async Task<bool> TakePayment(Guid id, int amount)
        {
            var query = Builders<Models.User>.Filter.Eq(x => x.Id, id) & Builders<Models.User>.Filter.Gte(x => x.Money, amount);
            var update = Builders<Models.User>.Update.Inc(x => x.Money, -amount);
            return await _dataContext.UpdateAsync(query, update);
        }
    }
}
