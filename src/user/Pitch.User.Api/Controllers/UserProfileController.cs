using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.User.Api.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pitch.User.Api.Controllers
{
    [Route("")]
    [Authorize]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /
        [HttpGet]
        public async Task<ActionResult<Models.User>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _userService.GetAsync(new Guid(userId));
        }
    }
}
