using System;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Store.API.Application.Events;
using Pitch.Store.API.Application.Subscribers;
using Pitch.Store.API.Infrastructure.Services;
using Xunit;

namespace Pitch.Store.API.Tests.Subscribers
{
    public class UserCreatedEventSubscriberTests
    {
        [Fact]
        public void Subscribe_SubscribeAsyncOnce()
        {
            // Arrange
            var mockBus = new Mock<IBus>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

            // Act
            var responder = new UserCreatedEventSubscriber(mockBus.Object, mockServiceScopeFactory.Object);
            responder.Subscribe();

            // Assert
            mockBus.Verify(x => x.SubscribeAsync("store", It.IsAny<Func<UserCreatedEvent, Task>>()), Times.Once);
        }

        [Fact]
        public async Task RedeemMatchRewards_CallsRedeemMatchRewardsOnce()
        {
            // Arrange
            var stubEvent = new UserCreatedEvent()
            {
                Id = Guid.NewGuid()
            };

            var mockBus = new Mock<IBus>();

            var mockPackService = new Mock<IPackService>();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IPackService))).Returns(mockPackService.Object);

            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(mockServiceScope.Object);

            // Act
            var subscriber = new UserCreatedEventSubscriber(mockBus.Object, mockServiceScopeFactory.Object);
            await subscriber.CreateStartingPacksAsync(stubEvent);

            // Assert
            mockPackService.Verify(x => x.CreateStartingPacksAsync(stubEvent.Id), Times.Once);
        }
    }
}
