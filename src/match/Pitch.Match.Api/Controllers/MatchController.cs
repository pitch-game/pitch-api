using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Models;
using Pitch.Match.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IEnumerable<Models.MatchListResult>> Get([FromQuery] int skip, [FromQuery] int? take)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _matchService.GetAllAsync(skip, take, new Guid(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> Get(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var match = await _matchService.GetAsync(id);
            match.AsOfNow();
            //return new MatchResult(match);
            return new { Match = new MatchResult(match), SubsRemaining = MatchService.SUB_COUNT - match.GetTeam(new Guid(userId)).UsedSubs };
        }

        [HttpGet("lineup")]
        public async Task<ActionResult<dynamic>> Lineup(Guid matchId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _matchService.GetLineupAsync(matchId, new Guid(userId));
        }

        [HttpPost("substitution")]
        public async Task<ActionResult> Substitution([FromBody]SubRequest req)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.Substitution(req.Off, req.On, req.MatchId, new Guid(userId));
            return Ok();
        }

        [HttpGet("claim")]
        public async Task Claim()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.ClaimAsync(new Guid(userId));
        }
    }
}
