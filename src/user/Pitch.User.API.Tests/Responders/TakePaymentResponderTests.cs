using System;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.User.API.Application.Requests;
using Pitch.User.API.Application.Responders;
using Pitch.User.API.Application.Responses;
using Pitch.User.API.Services;
using Xunit;

namespace Pitch.User.API.Tests.Responders
{
    public class TakePaymentResponderTests
    {
        [Fact]
        public void Register_CallsPublishAsyncOnce()
        {
            // Arrange
            var mockBus = new Mock<IBus>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

            // Act
            var responder = new TakePaymentResponder(mockBus.Object, mockServiceScopeFactory.Object);
            responder.Register();

            // Assert
            mockBus.Verify(x => x.RespondAsync(It.IsAny<Func<TakePaymentRequest, Task<TakePaymentResponse>>>()), Times.Once);
        }

        [Fact]
        public async Task Response_CallsGetOrCreateAsync()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "test@test.com";
            var stubRequest = new TakePaymentRequest()
            {
                UserId = userId,
                Amount = 10
            };

            var mockBus = new Mock<IBus>();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x.TakePayment(userId, 10)).ReturnsAsync(true);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(mockUserService.Object);

            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(mockServiceScope.Object);

            // Act
            var responder = new TakePaymentResponder(mockBus.Object, mockServiceScopeFactory.Object);
            var response = await responder.Response(stubRequest);

            // Assert
            mockUserService.Verify(x => x.TakePayment(userId, 10), Times.Once);
            Assert.NotNull(response);
            Assert.True(response.Success);
        }
    }
}
