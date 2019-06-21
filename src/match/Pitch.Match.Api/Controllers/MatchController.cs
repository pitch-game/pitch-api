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
    [Route("")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResult>> Get(Guid id)
        {
            var match = await _matchService.GetAsync(id);

            //TODO move to model
            match.Events = match.Events.Where(x => x.Minute < match.Duration).ToList();
            match.Statistics = match.Statistics.Where(x => x.Minute < match.Duration).ToList();

            var result = new MatchResult(match);
            result.Minute = match.Duration;
            result.Expired = match.IsOver;
            result.ExpiredOn = match.IsOver ? match.KickOff.AddMinutes(90 + match.ExtraTime) : (DateTime?)null;
            return result;
        }

        //TODO move to seasons service and model
        [HttpGet("unclaimed")]
        public async Task<dynamic> Unclaimed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return new { hasUnclaimed = await _matchService.HasUnclaimed(new Guid(userId)) };
        }

        [HttpGet("claim")]
        public async Task Claim()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.ClaimAsync(new Guid(userId));
        }
    }
}
