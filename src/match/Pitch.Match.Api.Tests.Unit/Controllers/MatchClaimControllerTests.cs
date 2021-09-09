using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Controllers;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit.Controllers
{
    public class MatchClaimControllerTests
    {
        [Fact]
        public async Task Claim_ReturnsOk()
        {
            var userId = Guid.NewGuid();
            var mockMatchService = new Mock<IMatchService>();

            var controller = new ClaimController(mockMatchService.Object);

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

            // Act
            var result = await controller.Claim();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}