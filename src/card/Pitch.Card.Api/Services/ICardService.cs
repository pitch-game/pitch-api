using Pitch.Card.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Models.Card>> GetAllAsync(CardRequestModel req, string userId);
        Task<IEnumerable<Models.Card>> GetAsync(IEnumerable<Guid> ids);
        Task<Models.Card> CreateCardAsync(CreateCardModel createCardReq);
    }
}