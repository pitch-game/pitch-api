using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.Store.API.Controllers;
using Pitch.Store.API.Infrastructure.Services;
using Xunit;

namespace Pitch.Store.API.Tests.Controllers
{
    public class PackControllerTests
    {
        [Fact]
        public async Task Get_CallsOnceGetAllFromPackService()
        {
            var userId = Guid.NewGuid();

            var mockPackService = new Mock<IPackService>();

            var controller = new PacksController(mockPackService.Object);

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
            mockPackService.Verify(x => x.GetAll(userId.ToString()), Times.Once);
        }

        [Fact]
        public async Task Buy_CallsOnceBuyOnPackService()
        {
            var userId = Guid.NewGuid();

            var mockPackService = new Mock<IPackService>();

            var controller = new PacksController(mockPackService.Object);

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
            var result = await controller.Buy();

            //Assert
            mockPackService.Verify(x => x.Buy(userId), Times.Once);
        }

        [Fact]
        public async Task Open_CallsOpenOnPackService()
        {
            var userId = Guid.NewGuid();
            var packId = Guid.NewGuid();

            var mockPackService = new Mock<IPackService>();

            var controller = new PacksController(mockPackService.Object);

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
            var result = await controller.Open(packId);

            //Assert
            mockPackService.Verify(x => x.Open(packId, userId.ToString()), Times.Once);
        }
    }
}
