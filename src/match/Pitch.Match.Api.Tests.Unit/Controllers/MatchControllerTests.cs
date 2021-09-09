using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Controllers;
using Pitch.Match.API.Models;
using Xunit;

namespace Pitch.Match.API.Tests.Controllers
{
    public class MatchControllerTests
    {
        private MatchController _matchControllerStub;
        private Guid _matchId;

        private void Setup()
        {
            _matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var matchStub = new ApplicationCore.Models.Match.Match
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

            var mockMatchService = new Mock<IMatchService>();
            mockMatchService.Setup(x => x.GetAsAtElapsedAsync(_matchId)).Returns(Task.FromResult(matchStub));

            var mockAutomapper = new Mock<IMapper>();
            mockAutomapper.Setup(x => x.Map<LineupModel>(It.IsAny<Lineup>())).Returns(new LineupModel());

            _matchControllerStub = new MatchController(mockMatchService.Object, mockAutomapper.Object);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextStub = new DefaultHttpContext();
            contextStub.User = claimsPrincipal;
            var controllerContextStub = new ControllerContext();
            controllerContextStub.HttpContext = contextStub;
            _matchControllerStub.ControllerContext = controllerContextStub;
        }

        [Fact]
        public async Task GetReturnsMatchListResultModel()
        {
            // Arrange
            Setup();

            // Act
            var result = await _matchControllerStub.Get(0, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<MatchListResultModel>>(result);
        }

        [Fact]
        public async Task GetReturnsMatchModel()
        {
            // Arrange
            Setup();

            // Act
            var result = await _matchControllerStub.Get(_matchId);

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
            var result = await _matchControllerStub.Lineup(_matchId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<LineupModel>(result.Value);
        }

        [Fact]
        public async Task SubstitutionReturnsOk()
        {
            // Arrange
            Setup();

            // Act
            var result = await _matchControllerStub.Substitution(_matchId, new SubRequest());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}