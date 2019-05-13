using EasyNetQ;
using Pitch.Player.Consumer.Messages;

namespace Pitch.Player.Consumer.Handlers
{
    public class PlayerRequestResponder
    {
        private readonly IBus _bus;
        public PlayerRequestResponder(IBus bus)
        {
            _bus = bus;
        }
        public void Subscribe()
        {
            _bus.Respond<PlayerRequest, TestResponse>(Response);
        }
        private TestResponse Response(PlayerRequest request)
        {
            return new TestResponse();
        }
    }

    public class TestResponse { }
}
