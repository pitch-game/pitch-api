using Pitch.Card.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitch.Card.API.Infrastructure.Services
{
    public interface ICardService
    {
        Task<Models.Card> GetAsync(Guid id);
        Task<IEnumerable<Models.Card>> GetAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<Models.Card>> GetAllAsync(CardRequestModel req, string userId);
        Task<Models.Card> CreateCardAsync(CreateCardModel createCardReq);
        Task SetGoals(IDictionary<Guid, int> scorers);
    }
}