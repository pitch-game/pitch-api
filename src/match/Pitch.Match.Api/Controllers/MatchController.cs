using Microsoft.AspNetCore.Mvc;
using Pitch.Match.Api.Application.Engine;
using Pitch.Match.Api.Models;
using Pitch.Match.Api.Services;
using System;

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
        public ActionResult<Models.Match> Get(Guid id)
        {
            var session = _matchmakingService.GetSession(id);

            var squad1 = session.HostPlayerId;
            var squad2 = session.JoinedPlayerId.Value;

            var match = new Models.Match()
            {
                Id = session.Id,
                KickOff = DateTime.Now,
                User1Id = session.HostPlayerId,
                User2Id = session.JoinedPlayerId.Value,
                Team1 = new Squad()
                {
                    Id = Guid.NewGuid(),
                    Lineup = null //get from service;
                },
                Team2 = new Squad()
                {
                    Id = Guid.NewGuid(),
                    Lineup = null //get from service;
                }
            };

            return _matchService.SimulateReentrant(match);
        }
    }
}
