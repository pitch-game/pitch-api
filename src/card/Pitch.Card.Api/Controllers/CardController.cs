using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pitch.Card.API.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Pitch.Card.API.Models;
using System.Linq;

namespace Pitch.Card.API.Controllers
{
    [Authorize]
    [Route("")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        //GET /
        [HttpGet]
        public async Task<IEnumerable<Models.Card>> Get([FromQuery] int skip, [FromQuery] int? take, [FromQuery] string position, [FromQuery] string notIn = null)
        {
            var req = new CardRequestModel() { Skip = skip, Take = take ?? 10, PositionPriority = position, NotIn = notIn?.Split(";")?.Select(x => new Guid(x))};
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //TODO move to currentUserContext
            return await _cardService.GetAllAsync(req, userId);
        }

        //GET /1922b2d1-38db-4180-a29b-1b7dbbf94647
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Card>> Get(Guid id)
        {
            var cards = await _cardService.GetAsync(new[] { id });
            return cards.FirstOrDefault();
        }

        [HttpGet("cards/{ids}")]
        public async Task<IEnumerable<Models.Card>> Get(string ids)
        {
            var guids = ids.Split(";").Select(x => new Guid(x));
            return await _cardService.GetAsync(guids);
        }
    }
}
