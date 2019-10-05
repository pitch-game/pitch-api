using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pitch.User.API.Controllers;
using Pitch.User.API.Services;
using Xunit;

namespace Pitch.User.API.Tests.Controllers
{
   public class UserProfileControllerTests
    {
        [Fact]
        public async Task Get_ReturnsUserProfileFromMatchService()
        {
            var userId = Guid.NewGuid();
            var stubUserProfile = new Models.User()
            {
                Id = userId
            };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(x => x.GetAsync(userId)).Returns(Task.FromResult(stubUserProfile));

            var controller = new UserProfileController(mockUserService.Object);

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
            Assert.Equal(stubUserProfile, result.Value);
        }
    }
}
