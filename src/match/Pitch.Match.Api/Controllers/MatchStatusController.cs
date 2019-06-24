using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Models;
using Pitch.Match.Api.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Controllers
{
    [Authorize]
    [Route("status")]
    [ApiController]
    public class MatchStatusController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchStatusController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<MatchStatusResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _matchService.GetMatchStatus(new Guid(userId));
        }
    }
}
