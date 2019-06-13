using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Pitch.Store.Api.Supporting
{
    public class ScopedMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedMessageDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = (TConsumer)_serviceProvider.GetService(typeof(TConsumer));
                consumer.Consume(message);
            }
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = (TConsumer)_serviceProvider.GetService(typeof(TConsumer));
                await consumer.ConsumeAsync(message);
            }
        }
    }
}
