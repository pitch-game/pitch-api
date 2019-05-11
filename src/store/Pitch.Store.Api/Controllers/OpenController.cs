using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitch.Store.Api.Infrastructure.Services;

namespace Pitch.Store.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OpenController : ControllerBase
    {

        private readonly IPackService _packService;

        // GET open/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Open(Guid id)
        {
            return await _packService.Open(id);
        }
    }
}
