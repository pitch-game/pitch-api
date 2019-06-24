using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Services;
using System;

namespace Pitch.Match.Api.Controllers
{
    [Authorize]
    [Route("matchmaking")]
    [ApiController]
    public class MatchMakingController : ControllerBase
    {
        private readonly IMatchmakingService _matchmakingService;

        public MatchMakingController(IMatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        [HttpGet("validate/{id}")]
        public ActionResult<dynamic> Validate(Guid id)
        {
            var session = _matchmakingService.GetSession(id);
            return new { Valid = session != null && !session.Expired && session.Open }; //TODO move to session.isValid
        }

        [HttpGet("cancel/{id}")]
        public ActionResult Cancel(Guid id)
        {
            _matchmakingService.Cancel(id);
            return Ok();
        }
    }
}
