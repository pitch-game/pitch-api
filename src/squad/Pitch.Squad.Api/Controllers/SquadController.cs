using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Squad.Api.Services;

namespace Pitch.Squad.Api.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class SquadController : ControllerBase
    {
        private readonly ISquadService _squadService;
        private readonly IChemistryService _chemistryService;

        public SquadController(ISquadService activeSquadService, IChemistryService chemistryService)
        {
            _squadService = activeSquadService;
            _chemistryService = chemistryService;
        }

        // GET /
        [HttpGet]
        public async Task<Models.Squad> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            var squad = await _squadService.GetOrCreateAsync(userId);
            _chemistryService.SetChemistry(squad);
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
