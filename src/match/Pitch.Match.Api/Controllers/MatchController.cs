using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Application.Engine;
using Pitch.Match.Api.Models;
using Pitch.Match.Api.Services;
using System;
using System.Linq;

namespace Pitch.Match.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchEngine _matchService;
        private readonly IMatchmakingService _matchmakingService;

        public MatchController(IMatchEngine matchService, IMatchmakingService matchmakingService)
        {
            _matchService = matchService;
            _matchmakingService = matchmakingService;
        }

        [HttpGet("{id}")]
        public ActionResult<Models.MatchResult> Get(Guid id)
        {
            var session = _matchmakingService.GetSession(id);

            var squad1 = session.HostPlayerId;
            var squad2 = session.JoinedPlayerId.Value;

            var match = new Models.Match()
            {
                Id = session.Id,
                KickOff = DateTime.Now,
                HomeUserId = session.HostPlayerId,
                AwayUserId = session.JoinedPlayerId.Value,
                HomeTeam = new Squad()
                {
                    Id = Guid.NewGuid(),
                    Lineup = null //get from service;
                },
                AwayTeam = new Squad()
                {
                    Id = Guid.NewGuid(),
                    Lineup = null //get from service;
                }
            };

            //result will be simulated once per sub/tactical change
            var result = _matchService.SimulateReentrant(match);
            var CURRENT_MINUTE = 45;

            //stream events by minute
            result.Events = result.Events.Where(x => x.Minute < CURRENT_MINUTE).ToList();

            return new MatchResult(result);
        }
    }
}
