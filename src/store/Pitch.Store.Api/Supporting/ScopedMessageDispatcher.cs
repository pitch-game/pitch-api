using EasyNetQ.AutoSubscribe;
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
            var consumer = (TConsumer)_serviceProvider.GetService(typeof(TConsumer));
            try
            {
                consumer.Consume(message);
            }
            finally
            {
                //_container.Release(consumer);
            }
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message)
        {
            var consumer = (TConsumer)_serviceProvider.GetService(typeof(TConsumer));
            await consumer.ConsumeAsync(message);//.ContinueWith(t => _container.Release(consumer));
        }
    }
}
