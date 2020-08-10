using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Squad.API.Services;

namespace Pitch.Squad.API.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class SquadController : ControllerBase
    {
        private readonly ISquadService _squadService;

        public SquadController(ISquadService squadService)
        {
            _squadService = squadService;
        }

        [HttpGet]
        public async Task<Models.Squad> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var squad = await _squadService.GetOrCreateAsync(userId);
            return squad;
        }

        [HttpPut]
        public async Task<Models.Squad> Update(Models.Squad squad)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _squadService.UpdateAsync(squad, userId);
        }
    }
}
