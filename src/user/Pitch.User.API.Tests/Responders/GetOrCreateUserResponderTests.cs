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
    public class GetOrCreateUserResponderTests
    {
        [Fact]
        public void Register_CallsPublishAsyncOnce()
        {
            // Arrange
            var mockBus = new Mock<IBus>();
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

            // Act
            var responder = new GetOrCreateUserResponder(mockBus.Object, mockServiceScopeFactory.Object);
            responder.Register();

            // Assert
            mockBus.Verify(x => x.RespondAsync(It.IsAny<Func<GetOrCreateUserRequest, Task<GetOrCreateUserResponse>>>()), Times.Once);
        }

        [Fact]
        public async Task Response_CallsGetOrCreateAsync()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var email = "test@test.com";
            var stubRequest = new GetOrCreateUserRequest()
            {
                Email = email
            };

            var mockBus = new Mock<IBus>();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x.GetOrCreateAsync(email)).ReturnsAsync(new Models.User()
            {
                Id = userId,
                Email = email
            });

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(mockUserService.Object);

            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(mockServiceScope.Object);

            // Act
            var responder = new GetOrCreateUserResponder(mockBus.Object, mockServiceScopeFactory.Object);
            var response = await responder.Response(stubRequest);

            // Assert
            mockUserService.Verify(x => x.GetOrCreateAsync(email), Times.Once);
            Assert.NotNull(response);
            Assert.Equal(userId, response.Id);
        }
    }
}
