using Pitch.Squad.Api.Infrastructure.Repositories;
using Pitch.Squad.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Services
{
    internal class SquadService : ISquadService
    {
        private readonly ISquadRepository _squadRepository;
        private readonly ISquadValidationService _squadValidationService;

        public SquadService(ISquadRepository squadRepository, ISquadValidationService squadValidationService)
        {
            _squadRepository = squadRepository;
            _squadValidationService = squadValidationService;
        }

        public async Task<Models.Squad> GetOrCreateAsync(string userId)
        {
            var activeSquad = await _squadRepository.GetAsync(userId);
            return activeSquad ?? await _squadRepository.CreateAsync(userId);
        }

        public async Task<Models.Squad> UpdateAsync(Models.Squad squad, string userId)
        {
            var squadInDb = await _squadRepository.GetAsync(userId);
            if (!await _squadValidationService.Validate(squad, squadInDb.Id, userId)) throw new System.Exception("Squad is not valid.");
            return await _squadRepository.UpdateAsync(squad);
        }
    }
}
