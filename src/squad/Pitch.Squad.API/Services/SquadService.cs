using Pitch.Squad.API.Infrastructure.Repositories;
using System.Threading.Tasks;
using Pitch.Squad.API.Exceptions;

namespace Pitch.Squad.API.Services
{
    public interface ISquadService
    {
        Task<Models.Squad> GetOrCreateAsync(string userId);
        Task<Models.Squad> UpdateAsync(Models.Squad squad, string userId);
    }

    public class SquadService : ISquadService
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
            if (!await _squadValidationService.Validate(squad, squadInDb.Id, userId)) throw new InvalidSquadException("Squad is not valid.");
            return await _squadRepository.UpdateAsync(squad);
        }
    }
}
