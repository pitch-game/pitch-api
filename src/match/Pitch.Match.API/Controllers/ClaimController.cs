using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Pitch.Match.API.ApplicationCore.Services;

namespace Pitch.Match.API.Controllers
{
    [Authorize]
    [Route("claim")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public ClaimController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        public async Task<ActionResult> Claim()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.ClaimAsync(new Guid(userId));
            return Ok();
        }
    }
}
