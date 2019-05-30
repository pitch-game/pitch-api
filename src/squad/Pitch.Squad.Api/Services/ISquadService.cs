using System.Threading.Tasks;
using Pitch.Squad.Api.Models;

namespace Pitch.Squad.Api.Services
{
    public interface ISquadService
    {
        Task<Models.Squad> GetOrCreateAsync(string userId);
    }
}