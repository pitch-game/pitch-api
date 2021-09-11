using System;
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

namespace Pitch.Match.Api.Tests.Unit.Services
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

            var mockMatchSessionService = new Mock<IMatchSessionService>();

            var service = new MatchmakingService(mockMatchRepository.Object, mockMatchSessionService.Object);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Matchmake(userId));
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

            var matchSessionService = new MatchSessionService();
            matchSessionService.Sessions = sessions;

            var service = new MatchmakingService(mockMatchRepository.Object, matchSessionService);

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

            var matchSessionService = new MatchSessionService();
            matchSessionService.Sessions = sessions;

            var mockBus = new Mock<IBus>();
            var service = new MatchmakingService(mockMatchRepository.Object, matchSessionService);

            // Act
            var session = service.GetSession(sessionId);

            // Assert
            Assert.Equal(session, sessions.First());
        }

        [Fact]
        public void JoinSession_On_Existing_Session_CreateNewSession()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new()
                {
                    Id = sessionId,
                    HostPlayerId = Guid.NewGuid(),
                    JoinedPlayerId = Guid.NewGuid()
                }
            };

            var matchSessionService = new MatchSessionService();
            matchSessionService.Sessions = sessions;

            var service = new MatchmakingService(mockMatchRepository.Object, matchSessionService);

            _ = service.Matchmake(userId);

            sessions.Should().HaveCount(2);
        }

        [Fact]
        public void JoinSession_On_Existing_Session_JoinsSession()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new()
                {
                    Id = sessionId,
                    HostPlayerId = Guid.NewGuid(),
                    CreatedOn = DateTime.Now
                }
            };

            var matchSessionService = new MatchSessionService();
            matchSessionService.Sessions = sessions;

            var service = new MatchmakingService(mockMatchRepository.Object, matchSessionService);

            _ = service.Matchmake(userId);

            sessions.Should().HaveCount(1);
        }

        [Fact]
        public void JoinSession_AsHost_ThrowsException()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockMatchRepository = new Mock<IMatchRepository>();

            IList<MatchmakingSession> sessions = new List<MatchmakingSession>()
            {
                new()
                {
                    Id = sessionId,
                    HostPlayerId = userId
                }
            };

            var matchSessionService = new MatchSessionService();
            matchSessionService.Sessions = sessions;

            var service = new MatchmakingService(mockMatchRepository.Object, matchSessionService);

            Assert.ThrowsAsync<HubException>(() => service.Matchmake(userId));
        }
    }
}
