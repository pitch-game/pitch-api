using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Services;

namespace Pitch.Store.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OpenController : ControllerBase
    {

        private readonly IPackService _packService;

        public OpenController(IPackService packService)
        {
            _packService = packService;
        }

        // GET open/5
        [HttpGet("{id}")]
        public async Task<CreateCardResponse> Open(Guid id)
        {
            return await _packService.Open(id);
        }
    }
}
