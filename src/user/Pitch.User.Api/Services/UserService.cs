using Pitch.User.Api.Infrastructure.Repositories;
using Pitch.User.Api.Models;
using System;
using System.Threading.Tasks;

namespace Pitch.User.Api.Services
{
    public interface IUserService
    {
        Task<Models.User> GetAsync(Guid id);
        Task<Models.User> GetAsync(string email);
        Task<Models.User> GetOrCreateAsync(string email);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Models.User> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }

        public async Task<Models.User> GetAsync(string email)
        {
            return await _userRepository.GetAsync(email);
        }

        public async Task<Models.User> GetOrCreateAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            if(user == null)
            {
                user = await _userRepository.CreateAsync(email);
            }
            return user;
        }
    }
}
