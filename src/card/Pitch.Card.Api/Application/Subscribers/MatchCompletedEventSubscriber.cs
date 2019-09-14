using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Card.API.Application.Events;
using Pitch.Card.API.Infrastructure.Services;

namespace Pitch.Card.API.Application.Subscribers
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
            _bus.SubscribeAsync<MatchCompletedEvent>("card", async (@event) =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ICardService>().SetGoals(@event.Scorers);
                }
            });
        }
    }

}
