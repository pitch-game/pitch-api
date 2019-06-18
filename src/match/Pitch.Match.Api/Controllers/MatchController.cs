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

            //TODO move to model
            match.Events = match.Events.Where(x => x.Minute < match.Duration).ToList();
            match.Statistics = match.Statistics.Where(x => x.Minute < match.Duration).ToList();

            var result = new MatchResult(match);
            result.Minute = match.Duration;
            result.Expired = match.IsExpired;
            result.ExpiredOn = match.IsExpired ? match.KickOff.AddMinutes(90 + match.ExtraTime) : (DateTime?)null;
            return result;
        }
    }
}
