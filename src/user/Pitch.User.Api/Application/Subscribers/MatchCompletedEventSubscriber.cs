using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.User.Api.Application.Events;
using Pitch.User.Api.Services;

namespace Pitch.User.Api.Application.Subscribers
{
    public interface ISubscriber
    {
        void Subscribe();
    }
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
            _bus.SubscribeAsync<MatchCompletedEvent>("user", async (@event) =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<IUserService>().RedeemMatchRewards(@event.UserId, @event.Victorious);
                }
            });
        }
    }

}
