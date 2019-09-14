using System.Threading.Tasks;
using Pitch.Squad.API.Models;

namespace Pitch.Squad.API.Infrastructure.Repositories
{
    public interface ISquadRepository
    {
        Task<Models.Squad> GetAsync(string userId);
        Task<Models.Squad> CreateAsync(string userId);
        Task<Models.Squad> UpdateAsync(Models.Squad squad);
    }
}