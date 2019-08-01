using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Controllers
{
    [Authorize]
    [Route("claim")]
    [ApiController]
    public class MatchClaimController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchClaimController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        public async Task Claim()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.ClaimAsync(new Guid(userId));
        }
    }
}
