using System;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Services
{
    public interface ICardService
    {
        Task<Models.Card> GetAsync(Guid id);
        Task<Models.Card> CreateCardAsync();
    }
}