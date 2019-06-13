using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Pitch.Store.Api.Application.Events;

namespace Pitch.Store.Api.Application.Subscribers
{
    public class UserCreatedEventSubscriber : IConsumeAsync<UserCreatedEvent>
    {
        public async Task ConsumeAsync(UserCreatedEvent message)
        {
            //TODO add 11 packs, 1 for each position and 6 random for subs
            throw new System.NotImplementedException();
        }
    }
}
