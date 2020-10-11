using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Pitch.Match.API.ApplicationCore.Models.Matchmaking;
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

        [Fact]
        public void CreateSession_CreatesANewSession()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>();

            var mockMatchSessionService = new Mock<IMatchSessionService>();
            mockMatchSessionService.SetupGet(x => x.Sessions).Returns(sessions);
            mockMatchSessionService.SetupSet(x => x.Sessions).Callback(r => sessions = r);

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            // Act
            var session = service.CreateSession(userId);

            Assert.Equal(session.HostPlayerId, userId);
            Assert.Contains(session, sessions);
        }

        [Fact]
        public void Cancel_RemovesSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new MatchmakingSession
                {
                    Id = sessionId
                }
            };

            var mockMatchSessionService = new Mock<IMatchSessionService>();
            mockMatchSessionService.SetupGet(x => x.Sessions).Returns(sessions);
            mockMatchSessionService.SetupSet(x => x.Sessions).Callback(r => sessions = r);

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            // Act
            service.Cancel(sessionId);

            // Assert
            Assert.True(sessions.Count == 0);
        }

        [Fact]
        public void GetSession_ReturnsSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new MatchmakingSession
                {
                    Id = sessionId
                }
            };

            var mockMatchSessionService = new Mock<IMatchSessionService>();
            mockMatchSessionService.SetupGet(x => x.Sessions).Returns(sessions);
            mockMatchSessionService.SetupSet(x => x.Sessions).Callback(r => sessions = r);

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            // Act
            var session = service.GetSession(sessionId);

            // Assert
            Assert.Equal(session, sessions.First());
        }

        [Fact]
        public void JoinSession_ReturnsCompletedSession()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new MatchmakingSession
                {
                    Id = sessionId,
                    HostPlayerId = Guid.NewGuid()
                }
            };

            var mockMatchSessionService = new Mock<IMatchSessionService>();
            mockMatchSessionService.SetupGet(x => x.Sessions).Returns(sessions);
            mockMatchSessionService.SetupSet(x => x.Sessions).Callback(r => sessions = r);

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            var session = service.JoinSession(sessionId, userId);

            session.JoinedPlayerId.Should().Be(userId);
            session.Open.Should().BeFalse();
            session.CompletedOn.Should().BeCloseTo(DateTime.Now, 2000);
        }

        [Fact]
        public void JoinSession_AsHost_ThrowsException()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new MatchmakingSession
                {
                    Id = sessionId,
                    HostPlayerId = userId
                }
            };

            var mockMatchSessionService = new Mock<IMatchSessionService>();
            mockMatchSessionService.SetupGet(x => x.Sessions).Returns(sessions);
            mockMatchSessionService.SetupSet(x => x.Sessions).Callback(r => sessions = r);

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, mockBus.Object, mockMatchSessionService.Object);

            // Act & Assert
            Assert.Throws<HubException>(() => service.JoinSession(sessionId, userId));
        }
    }
}
