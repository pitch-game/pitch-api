using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        // GET card/1922b2d1-38db-4180-a29b-1b7dbbf94647
        [HttpGet("{id}")]
        public Task<ActionResult<Models.Card>> Get(Guid id)
        {
            return "value";
        }
    }
}
