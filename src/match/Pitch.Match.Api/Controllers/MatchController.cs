using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.ApplicationCore.Models;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Pitch.Match.Api.ApplicationCore.Services;

namespace Pitch.Match.Api.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;

        public MatchController(IMatchService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MatchListResultModel>> Get([FromQuery] int skip, [FromQuery] int? take)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var matchListResults = await _matchService.GetAllAsync(skip, take, new Guid(userId));
            return _mapper.Map<IEnumerable<MatchListResultModel>>(matchListResults);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchModel>> Get(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var match = await _matchService.GetAsync(id);
            match.AsOfNow();
            return new MatchModel { Match = new MatchResult(match), SubsRemaining = MatchService.SUB_COUNT - match.GetTeam(new Guid(userId)).UsedSubs };
        }

        [HttpGet("{matchId}/lineup")]
        public async Task<ActionResult<LineupModel>> Lineup([FromRoute]Guid matchId) //TODO model
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var lineup = await _matchService.GetLineupAsync(matchId, new Guid(userId));
            return _mapper.Map<LineupModel>(lineup);
        }

        [HttpPost("{matchId}/substitution")]
        public async Task<ActionResult> Substitution([FromRoute]Guid matchId, [FromBody]SubRequest req)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            await _matchService.Substitution(req.Off, req.On, matchId, new Guid(userId));
            return Ok();
        }
    }
}
