using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using FluentAssertions;
using Moq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Services;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Pitch.Match.API.ApplicationCore.Models.Matchmaking;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Infrastructure.MessageBus.Events;
using Pitch.Match.API.Infrastructure.MessageBus.Requests;
using Pitch.Match.API.Infrastructure.MessageBus.Responses;
using Pitch.Match.API.Infrastructure.Repositories;
using Xunit;

namespace Pitch.Match.API.Tests.Services
{
    public class MatchServiceTests
    {
        private MatchService _matchService;

        [Fact]
        public async Task ClaimingMatchReward_AsAwayTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var unclaimedMatch = new ApplicationCore.Models.Match.Match
            {
                Id = matchId,
                HomeTeam = new TeamDetails
                {
                    UserId = Guid.NewGuid(),
                    Squad = new Squad
                    {
                        Id = Guid.NewGuid()
                    },
                },
                AwayTeam = new TeamDetails
                {
                    UserId = userId,
                    Squad = new Squad
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            ApplicationCore.Models.Match.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<ApplicationCore.Models.Match.Match>) new List<ApplicationCore.Models.Match.Match>
                    {unclaimedMatch}));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Callback<ApplicationCore.Models.Match.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>()))
                .Callback<MatchCompletedEvent>(r => publishedEvent = r);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            await _matchService.ClaimAsync(userId);

            //Assert
            updatedMatch.AwayTeam.HasClaimedRewards.Should().BeTrue();
            publishedEvent.UserId.Should().Be(userId);
            publishedEvent.MatchId.Should().Be(matchId);
            publishedEvent.Victorious.Should().BeFalse();
            publishedEvent.Scorers.Should().BeEmpty();
        }

        [Fact]
        public async Task ClaimingMatchReward_AsHomeTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var unclaimedMatch = new ApplicationCore.Models.Match.Match
            {
                Id = matchId,
                HomeTeam = new TeamDetails
                {
                    UserId = userId,
                    Squad = new Squad
                    {
                        Id = Guid.NewGuid()
                    }
                },
                AwayTeam = new TeamDetails
                {
                    UserId = Guid.NewGuid(),
                    Squad = new Squad
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            ApplicationCore.Models.Match.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<ApplicationCore.Models.Match.Match>) new List<ApplicationCore.Models.Match.Match>
                    {unclaimedMatch}));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Callback<ApplicationCore.Models.Match.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>()))
                .Callback<MatchCompletedEvent>(r => publishedEvent = r);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            await _matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.HomeTeam.HasClaimedRewards);
            publishedEvent.UserId.Should().Be(userId);
            publishedEvent.MatchId.Should().Be(matchId);
            publishedEvent.Victorious.Should().BeFalse();
            publishedEvent.Scorers.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsMatch()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();

            var matchId = Guid.NewGuid();
            var match = new ApplicationCore.Models.Match.Match {Id = matchId};

            mockMatchRepository.Setup(x => x.GetAsync(matchId)).Returns(Task.FromResult(match));

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            var returnedMatch = await _matchService.GetAsAtElapsedAsync(matchId);

            //Assert
            Assert.Equal(returnedMatch, match);
            mockCalculatedStatService.Verify(x => x.Set(It.IsAny<ApplicationCore.Models.Match.Match>()), Times.Once);
        }

        [Fact]
        public async Task KickOff_ReturnsMatch()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            var hostPlayerId = Guid.NewGuid();
            var joinedPlayerId = Guid.NewGuid();

            var session = new MatchmakingSession
            {
                Id = sessionId,
                HostPlayerId = hostPlayerId,
                JoinedPlayerId = joinedPlayerId
            };
            mockMatchmakingService.Setup(x => x.GetSession(sessionId)).Returns(session);

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.Simulate(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Returns(new ApplicationCore.Models.Match.Match());

            ApplicationCore.Models.Match.Match simulatedMatch = null;

            var mockBus = new Mock<IBus>();
            var squadResponse = new GetSquadResponse
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Subs = new Card[0],
                Lineup = new Dictionary<string, Card>()
            };
            mockBus.Setup(x => x.RequestAsync<GetSquadRequest, GetSquadResponse>(It.Is<GetSquadRequest>(x => x.UserId == hostPlayerId || x.UserId == joinedPlayerId)))
                .Returns(Task.FromResult(squadResponse));

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.CreateAsync(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Callback<ApplicationCore.Models.Match.Match>(r => simulatedMatch = r);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            await _matchService.KickOff(sessionId);

            //Assert
            Assert.NotNull(simulatedMatch);
        }

        [Fact]
        public async Task Substitution_CallsMatchSubstituteMethodOnce_AndIncrementsSubCount()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var teamDetails = new TeamDetails()
            {
                UserId = userId,
                UsedSubs = 0
            };

            var mockMatch = new Mock<ApplicationCore.Models.Match.Match>();
            mockMatch.SetupGet(x => x.HomeTeam).Returns(teamDetails);
            mockMatch.SetupSet(x => x.HomeTeam).Callback(r => teamDetails = r);

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.Simulate(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Returns(mockMatch.Object);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(mockMatch.Object);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            await _matchService.Substitution(Guid.NewGuid(), Guid.NewGuid(), matchId, userId);

            mockMatch.Verify(x => x.Substitute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.Equal(1, teamDetails.UsedSubs);
        }

        [Fact]
        public async Task Substitution_WhenAllSubsUsed_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var mockMatch = new Mock<ApplicationCore.Models.Match.Match>();
            mockMatch.SetupGet(x => x.HomeTeam).Returns(new TeamDetails()
            {
                UserId = userId,
                UsedSubs = MatchService.SubCount
            });

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.Simulate(It.IsAny<ApplicationCore.Models.Match.Match>()))
                .Returns(mockMatch.Object);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(mockMatch.Object);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            await Assert.ThrowsAsync<Exception>(() => _matchService.Substitution(Guid.NewGuid(), Guid.NewGuid(), matchId, userId));
        }

        [Fact]
        public async Task GetMatchStatus_ReturnsCorrectModel()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var mockMatch = new Mock<ApplicationCore.Models.Match.Match>();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.HasUnclaimedAsync(userId)).ReturnsAsync(true);
            mockMatchRepository.Setup(x => x.GetInProgressAsync(userId)).ReturnsAsync((Guid?)null);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            var result = await _matchService.GetMatchStatus(userId);

            //Assert
            Assert.True(result.HasUnclaimedRewards);
            Assert.Null(result.InProgressMatchId);
        }

        [Fact]
        public async Task GetAllAsync_CallsGetAllAsyncOnce()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), userId)).ReturnsAsync(new List<ApplicationCore.Models.Match.Match>());

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            var result = await _matchService.GetAllAsync(0, 1, userId);

            //Assert
            mockMatchRepository.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), userId), Times.Once);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLineupAsync_CallsGetAsyncOnce()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var mockMatch = new Mock<ApplicationCore.Models.Match.Match>();
            mockMatch.SetupGet(x => x.HomeTeam).Returns(new TeamDetails()
            {
                UserId = userId,
                Squad = new Squad()
            });

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(mockMatch.Object);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object);

            //Act
            var result = await _matchService.GetLineupAsync(matchId, userId);

            //Assert
            mockMatchRepository.Verify(x => x.GetAsync(matchId), Times.Once);
        }
    }
}