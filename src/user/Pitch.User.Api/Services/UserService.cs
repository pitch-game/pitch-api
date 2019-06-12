using Pitch.User.Api.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Pitch.User.Api.Services
{
    public interface IUserService
    {
        Task<Models.User> GetAsync(Guid id);
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
            return await _userRepository.Get(id);
        }
    }
}
