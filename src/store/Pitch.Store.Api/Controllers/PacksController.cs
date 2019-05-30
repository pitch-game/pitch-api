using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Services;

namespace Pitch.Store.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PacksController : ControllerBase
    {

        private readonly IPackService _packService;

        public PacksController(IPackService packService)
        {
            _packService = packService;
        }

        // GET open/5
        [HttpGet("open/{id}")]
        public async Task<CreateCardResponse> Open(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _packService.Open(id, userId);
        }

        // GET buy
        [HttpPost("buy")]
        public async Task<Guid> Buy()
        {
            return await _packService.Buy();
        }
    }
}
