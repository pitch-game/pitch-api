using System;
using System.Threading.Tasks;
using EasyNetQ;
using Moq;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Infrastructure.Repositories;
using Xunit;

namespace Pitch.Match.API.Tests.Services
{
    public class MatchmakingServiceTests
    {
        [Fact]
        public async Task Matchmaking_WhenAlreadySimulatingAGame_ShouldThrowException()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetInProgressAsync(userId)).Returns(Task.FromResult<Guid?>(Guid.NewGuid()));

            var mockBus = new Mock<IBus>();
            var mockMatchSessionService = new Mock<IMatchSessionService>();

            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Matchmake(userId));
        }
    }
}
