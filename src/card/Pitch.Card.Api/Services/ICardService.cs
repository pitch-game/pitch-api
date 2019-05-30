using Pitch.Card.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Models.Card>> GetAllAsync(string userId);
        Task<Models.Card> GetAsync(Guid id);
        Task<Models.Card> CreateCardAsync(CreateCardModel createCardReq);
    }
}