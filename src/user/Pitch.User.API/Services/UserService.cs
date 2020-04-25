using EasyNetQ;
using Pitch.User.API.Application.Events;
using Pitch.User.API.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Pitch.User.API.Services
{
    public interface IUserService
    {
        Task<Models.User> GetAsync(Guid id);
        Task<Models.User> GetAsync(string email);
        Task<Models.User> GetOrCreateAsync(string email);
        Task RedeemMatchRewards(Guid id, bool victorious);
        Task<bool> TakePayment(Guid id, int amount);
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
                await _bus.PublishAsync(new UserCreatedEvent(user.Id));
            }
            return user;
        }

        public async Task RedeemMatchRewards(Guid id, bool victorious)
        {
            var user = await _userRepository.GetAsync(id);
            //TODO race condition?
            user.XP += 1000;
            user.Money += victorious ? 15000 : 10000;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> TakePayment(Guid id, int amount)
        {
            return await _userRepository.TakePayment(id, amount);
        }
    }
}
