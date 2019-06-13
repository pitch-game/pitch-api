using EasyNetQ;
using Pitch.User.Api.Application.Events;
using Pitch.User.Api.Infrastructure.Repositories;
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
        private readonly IBus _bus;

        public UserService(IUserRepository userRepository, IBus bus)
        {
            _userRepository = userRepository;
            _bus = bus;
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
                _bus.Publish(new UserCreatedEvent(user.Id));
            }
            return user;
        }
    }
}
