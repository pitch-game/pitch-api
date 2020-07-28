using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Store.API.Application.Events;
using Pitch.Store.API.Services;

namespace Pitch.Store.API.Application.Subscribers
{
    public interface IUserCreatedEventSubscriber
    {
        Task CreateStartingPacksAsync(UserCreatedEvent @event);
    }

    public class UserCreatedEventSubscriber : ISubscriber, IUserCreatedEventSubscriber
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserCreatedEventSubscriber(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Subscribe()
        {
            _bus.SubscribeAsync<UserCreatedEvent>("store", CreateStartingPacksAsync);
        }

        public async Task CreateStartingPacksAsync(UserCreatedEvent @event)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<IPackService>().CreateStartingPacksAsync(@event.Id);
            }
        }
    }
}
