using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Card.Api.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        //GET card/1922b2d1-38db-4180-a29b-1b7dbbf94647
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Card>> Get(Guid id)
        {
            return await _cardService.GetAsync(id);
        }
    }
}
