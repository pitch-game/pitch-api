using System.Threading.Tasks;
using Pitch.Squad.Api.Models;

namespace Pitch.Squad.Api.Infrastructure.Repositories
{
    internal interface ISquadRepository
    {
        Task<Models.Squad> GetAsync(string userId);
        Task<Models.Squad> CreateAsync(string userId);
        Task<Models.Squad> UpdateAsync(Models.Squad squad);
    }
}