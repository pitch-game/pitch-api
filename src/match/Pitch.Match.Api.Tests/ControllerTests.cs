using Moq;
using System;
using Xunit;
using System.Threading.Tasks;
using Pitch.Match.Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Models;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Pitch.Match.Api.ApplicationCore.Services;

namespace Pitch.Match.Api.Tests
{
    public class ControllerTests
    {
        private MatchController matchControllerStub;
        private Guid matchId;

        private void Setup()
        {
            matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var matchStub = new ApplicationCore.Models.Match()
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

            var mockMatchService = new Mock<IMatchService>();
            mockMatchService.Setup(x => x.GetAsync(matchId)).Returns(Task.FromResult(matchStub));

            var mockAutomapper = new Mock<IMapper>();
            mockAutomapper.Setup(x => x.Map<LineupModel>(It.IsAny<ApplicationCore.Models.Lineup>())).Returns(new LineupModel());

            matchControllerStub = new MatchController(mockMatchService.Object, mockAutomapper.Object);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextStub = new DefaultHttpContext();
            contextStub.User = claimsPrincipal;
            var controllerContextStub = new ControllerContext();
            controllerContextStub.HttpContext = contextStub;
            matchControllerStub.ControllerContext = controllerContextStub;
        }

        [Fact]
        public async Task GetReturnsMatchModel()
        {
            // Arrange
            Setup();

            // Act
            var result = await matchControllerStub.Get(matchId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<MatchModel>(result.Value);
        }

        [Fact]
        public async Task LineupReturnsLineupModel()
        {
            // Arrange
            Setup();

            // Act
            var result = await matchControllerStub.Lineup(matchId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<LineupModel>(result.Value);
        }
    }
}
