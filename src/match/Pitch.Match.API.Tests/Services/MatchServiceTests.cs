using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using Moq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Models;
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

            var unclaimedMatch = new ApplicationCore.Models.Match
            {
                HomeTeam = new TeamDetails
                {
                    UserId = Guid.NewGuid(),
                    Squad = new Squad
                    {
                        Id = Guid.NewGuid()
                    }
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

            ApplicationCore.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<ApplicationCore.Models.Match>) new List<ApplicationCore.Models.Match>
                    {unclaimedMatch}));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match>()))
                .Callback<ApplicationCore.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>()))
                .Callback<MatchCompletedEvent>(r => publishedEvent = r);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

            //Act
            await _matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.AwayTeam.HasClaimedRewards);
            Assert.NotNull(publishedEvent);
        }

        [Fact]
        public async Task ClaimingMatchReward_AsHomeTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var userId = Guid.NewGuid();

            var unclaimedMatch = new ApplicationCore.Models.Match
            {
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

            ApplicationCore.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<ApplicationCore.Models.Match>) new List<ApplicationCore.Models.Match>
                    {unclaimedMatch}));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match>()))
                .Callback<ApplicationCore.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>()))
                .Callback<MatchCompletedEvent>(r => publishedEvent = r);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

            //Act
            await _matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.HomeTeam.HasClaimedRewards);
            Assert.NotNull(publishedEvent);
        }

        [Fact]
        public async Task Get_ReturnsMatch()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();

            var matchId = Guid.NewGuid();
            var match = new ApplicationCore.Models.Match {Id = matchId};

            mockMatchRepository.Setup(x => x.GetAsync(matchId)).Returns(Task.FromResult(match));

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

            //Act
            var returnedMatch = await _matchService.GetAsync(matchId);

            //Assert
            Assert.Equal(returnedMatch, match);
        }

        [Fact]
        public async Task KickOff_ReturnsMatch()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            var session = new MatchmakingSession
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                JoinedPlayerId = Guid.NewGuid()
            };
            mockMatchmakingService.Setup(x => x.GetSession(sessionId)).Returns(session);

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.SimulateReentrant(It.IsAny<ApplicationCore.Models.Match>()))
                .Returns(new ApplicationCore.Models.Match());

            ApplicationCore.Models.Match simulatedMatch = null;

            var mockBus = new Mock<IBus>();
            var squadResponse = new GetSquadResponse
            {
                Lineup = new Dictionary<string, Card>()
            };
            mockBus.Setup(x => x.RequestAsync<GetSquadRequest, GetSquadResponse>(It.IsAny<GetSquadRequest>()))
                .Returns(Task.FromResult(squadResponse));

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.CreateAsync(It.IsAny<ApplicationCore.Models.Match>()))
                .Callback<ApplicationCore.Models.Match>(r => simulatedMatch = r);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

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

            var mockMatch = new Mock<ApplicationCore.Models.Match>();
            mockMatch.SetupGet(x => x.HomeTeam).Returns(teamDetails);
            mockMatch.SetupSet(x => x.HomeTeam).Callback(r => teamDetails = r);

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.SimulateReentrant(It.IsAny<ApplicationCore.Models.Match>()))
                .Returns(mockMatch.Object);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(mockMatch.Object);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

            await _matchService.Substitution(Guid.NewGuid(), Guid.NewGuid(), matchId, userId);

            mockMatch.Verify(x => x.Substitute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.Equal(1, teamDetails.UsedSubs);
        }

        [Fact]
        public async Task Substitution_WhenAllSubsUsed_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var mockMatch = new Mock<ApplicationCore.Models.Match>();
            mockMatch.SetupGet(x => x.HomeTeam).Returns(new TeamDetails()
            {
                UserId = userId,
                UsedSubs = MatchService.SUB_COUNT
            });

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.SimulateReentrant(It.IsAny<ApplicationCore.Models.Match>()))
                .Returns(mockMatch.Object);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(mockMatch.Object);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object);

            await Assert.ThrowsAsync<Exception>(() => _matchService.Substitution(Guid.NewGuid(), Guid.NewGuid(), matchId, userId));
        }
    }
}