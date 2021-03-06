﻿using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Store.API.Application.Events;
using Pitch.Store.API.Services;

namespace Pitch.Store.API.Application.Subscribers
{
    public interface IMatchCompletedEventSubscriber
    {
        Task RedeemMatchRewards(MatchCompletedEvent @event);
    }

    public class MatchCompletedEventSubscriber : IMatchCompletedEventSubscriber, ISubscriber
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
            _bus.SubscribeAsync<MatchCompletedEvent>("store", RedeemMatchRewards);
        }

        public async Task RedeemMatchRewards(MatchCompletedEvent @event)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<IPackService>().RedeemMatchRewards(@event.UserId, @event.Victorious);
            }
        }
    }
}
