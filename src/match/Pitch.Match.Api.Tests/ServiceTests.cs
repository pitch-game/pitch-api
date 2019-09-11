using EasyNetQ;
using Moq;
using Pitch.Match.Api.ApplicationCore.Engine;
using Pitch.Match.Api.ApplicationCore.Modelsmaking;
using Pitch.Match.Api.Infrastructure.MessageBus.Events;
using Pitch.Match.Api.Infrastructure.MessageBus.Requests;
using Pitch.Match.Api.Infrastructure.MessageBus.Responses;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class ServiceTests
    {
        private MatchService matchService;

        [Fact]
        public async Task ClaimingMatchReward_AsHomeTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var userId = Guid.NewGuid();

            var unclaimedMatch = new ApplicationCore.Models.Match()
            {
                HomeTeam = new ApplicationCore.Models.TeamDetails()
                {
                    UserId = userId,
                    Squad = new ApplicationCore.Models.Squad()
                    {
                        Id = Guid.NewGuid()
                    }
                },
                AwayTeam = new ApplicationCore.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid(),
                    Squad = new ApplicationCore.Models.Squad()
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            ApplicationCore.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(Task.FromResult((IEnumerable<ApplicationCore.Models.Match>)new List<ApplicationCore.Models.Match>() { unclaimedMatch }));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match>())).Callback<ApplicationCore.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>())).Callback<MatchCompletedEvent>(r => publishedEvent = r);

            matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object, mockMatchRepository.Object, mockBus.Object);

            //Act
            await matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.HomeTeam.HasClaimedRewards);
            Assert.NotNull(publishedEvent);
        }

        [Fact]
        public async Task ClaimingMatchReward_AsAwayTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var userId = Guid.NewGuid();

            var unclaimedMatch = new ApplicationCore.Models.Match()
            {
                HomeTeam = new ApplicationCore.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid(),
                    Squad = new ApplicationCore.Models.Squad()
                    {
                        Id = Guid.NewGuid()
                    }
                },
                AwayTeam = new ApplicationCore.Models.TeamDetails()
                {
                    UserId = userId,
                    Squad = new ApplicationCore.Models.Squad()
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            ApplicationCore.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(Task.FromResult((IEnumerable<ApplicationCore.Models.Match>)new List<ApplicationCore.Models.Match>() { unclaimedMatch }));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match>())).Callback<ApplicationCore.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<MatchCompletedEvent>())).Callback<MatchCompletedEvent>(r => publishedEvent = r);

            matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object, mockMatchRepository.Object, mockBus.Object);

            //Act
            await matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.AwayTeam.HasClaimedRewards);
            Assert.NotNull(publishedEvent);
        }

        [Fact]
        public async Task TestGet()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var mockMatchRepository = new Mock<IMatchRepository>();

            var matchId = Guid.NewGuid();
            var match = new ApplicationCore.Models.Match() { Id = matchId };

            mockMatchRepository.Setup(x => x.GetAsync(matchId)).Returns(Task.FromResult(match));

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();

            matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object, mockMatchRepository.Object, mockBus.Object);

            //Act
            var returnedMatch = await matchService.GetAsync(matchId);

            //Assert
            Assert.Equal(returnedMatch, match);
        }

        [Fact]
        public async Task TestKickOff()
        {
            var mockMatchmakingService = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            var session = new MatchmakingSession()
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                JoinedPlayerId = Guid.NewGuid()
            };
            mockMatchmakingService.Setup(x => x.GetSession(sessionId)).Returns(session);

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.SimulateReentrant(It.IsAny<ApplicationCore.Models.Match>())).Returns(new ApplicationCore.Models.Match());

            ApplicationCore.Models.Match simulatedMatch = null;

            var mockBus = new Mock<IBus>();
            var squadResponse = new GetSquadResponse()
            {
                Lineup = new Dictionary<string, ApplicationCore.Models.Card>()
            };
            mockBus.Setup(x => x.RequestAsync<GetSquadRequest, GetSquadResponse>(It.IsAny<GetSquadRequest>())).Returns(Task.FromResult(squadResponse));

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.CreateAsync(It.IsAny<ApplicationCore.Models.Match>())).Callback<ApplicationCore.Models.Match>(r => simulatedMatch = r);

            matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object, mockMatchRepository.Object, mockBus.Object);

            //Act
            await matchService.KickOff(sessionId);

            //Assert
            Assert.NotNull(simulatedMatch);
        }
    }
}
