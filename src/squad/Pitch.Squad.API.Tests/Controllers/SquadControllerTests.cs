using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.Squad.API.Controllers;
using Pitch.Squad.API.Services;
using Xunit;

namespace Pitch.Squad.API.Tests.Controllers
{
    public class SquadControllerTests
    {
        private readonly Mock<ISquadService> _squadServiceMock;

        public SquadControllerTests()
        {
            _squadServiceMock = new Mock<ISquadService>();
        }

        [Fact]
        public async Task Return_Squad()
        {
            var userId = Guid.NewGuid();

            var fixture = new Fixture();
            var squad = fixture.Create<Models.Squad>();

            _squadServiceMock.Setup(x => x.GetOrCreateAsync(userId.ToString()))
                .ReturnsAsync(squad);

            var controller = new SquadController(_squadServiceMock.Object);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextStub = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = contextStub
            };

            var result = await controller.Get();

            result.Should().Be(squad);
        }

        [Fact]
        public async Task Update_Squad()
        {
            var userId = Guid.NewGuid();

            var fixture = new Fixture();
            var squad = fixture.Create<Models.Squad>();

            _squadServiceMock.Setup(x => x.GetOrCreateAsync(userId.ToString()))
                .ReturnsAsync(squad);

            var controller = new SquadController(_squadServiceMock.Object);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextStub = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = contextStub
            };

            await controller.Update(squad);

            _squadServiceMock.Verify(x => x.UpdateAsync(squad, userId.ToString()), Times.Once);
        }
    }
}
