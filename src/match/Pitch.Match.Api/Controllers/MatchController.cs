using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Models;
using Pitch.Match.Api.Services;
using System;
using System.Linq;
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

            var matchDuration = DateTime.Now.Subtract(match.KickOff).TotalMinutes;

            //stream events by minute
            match.Events = match.Events.Where(x => x.Minute < matchDuration).ToList();
            match.Statistics = match.Statistics.Where(x => x.Minute < matchDuration).ToList();

            var result = new MatchResult(match);
            result.Minute = (int)matchDuration;

            return result;
        }
    }
}
