using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Controllers;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit.Controllers
{
    public class MatchStatusControllerTests
    {
        [Fact]
        public async Task Get_ReturnsMatchResultFromMatchService()
        {
            var userId = Guid.NewGuid();
            var stubMatchResult = new MatchStatusResult();
            var mockMatchService = new Mock<IMatchService>();
            mockMatchService.Setup(x => x.GetMatchStatus(userId)).Returns(Task.FromResult(stubMatchResult));

            var controller = new StatusController(mockMatchService.Object);

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

            //Act
            var result = await controller.Get();

            //Assert
            Assert.Equal(stubMatchResult, result);
        }
    }
}