using Microsoft.AspNetCore.Mvc;
using Pitch.User.Api.Services;
using System;
using System.Threading.Tasks;

namespace Pitch.User.Api.Controllers
{
    [Route("")]
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
            var userId = new Guid(); //todo
            return await _userService.GetAsync(userId);
        }
    }
}
