using System.Threading.Tasks;
using Pitch.Squad.Api.Models;

namespace Pitch.Squad.Api.Infrastructure.Repositories
{
    public interface ISquadRepository
    {
        Task<Models.Squad> GetAsync(string userId);
        Task<Models.Squad> CreateAsync(string userId);
        Task<Models.Squad> UpdateAsync(Models.Squad squad);
    }
}