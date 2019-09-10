using EasyNetQ;
using Moq;
using Pitch.Match.Api.ApplicationCore.Engine;
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

            mockMatchRepository.Setup(x => x.GetUnclaimedAsync(userId)).Returns(Task.FromResult((IEnumerable<ApplicationCore.Models.Match>)new List<ApplicationCore.Models.Match>() { unclaimedMatch }));
            mockMatchRepository.Setup(x => x.UpdateAsync(It.IsAny<ApplicationCore.Models.Match>())).Callback<ApplicationCore.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();
            var mockBus = new Mock<IBus>();

            matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object, mockMatchRepository.Object, mockBus.Object);

            //Act
            await matchService.ClaimAsync(userId);

            //Assert
            Assert.True(updatedMatch.HomeTeam.HasClaimedRewards);
        }
    }
}
