using System;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.User.API.Application.Events;
using Pitch.User.API.Application.Subscribers;
using Pitch.User.API.Services;
using Xunit;

namespace Pitch.User.API.Tests.Subscribers
{
    public class MatchCompletedEventSubscriberTests
    {
        [Fact]
        public void Subscribe_SubscribeAsyncOnce()
        {
            // Arrange
            var mockBus = new Mock<IBus>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

            // Act
            var responder = new MatchCompletedEventSubscriber(mockBus.Object, mockServiceScopeFactory.Object);
            responder.Subscribe();

            // Assert
            mockBus.Verify(x => x.SubscribeAsync("user", It.IsAny<Func<MatchCompletedEvent, Task>>()), Times.Once);
        }

        [Fact]
        public async Task RedeemMatchRewards_CallsRedeemMatchRewardsOnce()
        {
            // Arrange
            var stubEvent = new MatchCompletedEvent()
            {
                Victorious = false,
                UserId = Guid.NewGuid()
            };

            var mockBus = new Mock<IBus>();

            var mockUserService = new Mock<IUserService>();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(mockUserService.Object);

            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(mockServiceScope.Object);

            // Act
            var subscriber = new MatchCompletedEventSubscriber(mockBus.Object, mockServiceScopeFactory.Object);
            await subscriber.RedeemMatchRewards(stubEvent);

            // Assert
            mockUserService.Verify(x => x.RedeemMatchRewards(stubEvent.UserId, stubEvent.Victorious), Times.Once);
        }
    }
}
