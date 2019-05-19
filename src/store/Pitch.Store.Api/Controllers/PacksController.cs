using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Services;

namespace Pitch.Store.Api.Controllers
{
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
            return await _packService.Open(id);
        }

        // GET buy
        [HttpPost("buy")]
        public async Task<Guid> Buy()
        {
            return await _packService.Buy();
        }
    }
}
