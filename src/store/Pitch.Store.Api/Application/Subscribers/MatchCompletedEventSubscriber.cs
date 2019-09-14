using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Store.API.Application.Events;
using Pitch.Store.API.Infrastructure.Services;

namespace Pitch.Store.API.Application.Subscribers
{
    public class MatchCompletedEventSubscriber : ISubscriber
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MatchCompletedEventSubscriber(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Subscribe()
        {
            _bus.SubscribeAsync<MatchCompletedEvent>("store", async (@event) => {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<IPackService>().RedeemMatchRewards(@event.UserId, @event.Victorious);
                }
            });
        }
    }
}
