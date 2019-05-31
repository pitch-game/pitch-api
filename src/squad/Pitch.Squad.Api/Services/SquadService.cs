using Pitch.Squad.Api.Infrastructure.Repositories;
using Pitch.Squad.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Services
{
    internal class SquadService : ISquadService
    {
        private readonly ISquadRepository _squadRepository;

        public SquadService(ISquadRepository squadRepository)
        {
            _squadRepository = squadRepository;
        }

        public async Task<Models.Squad> GetOrCreateAsync(string userId)
        {
            var activeSquad = await _squadRepository.GetAsync(userId);
            return activeSquad ?? await _squadRepository.CreateAsync(userId);
        }

        public async Task<Models.Squad> UpdateAsync(Models.Squad squad, string userId)
        {
            if (!await ValidateSquad(squad, userId)) throw new System.Exception("Squad formation is invalid.");
            return await _squadRepository.UpdateAsync(squad);
        }

        private async Task<bool> ValidateSquad(Models.Squad squad, string userId) //TODO move to validation service
        {
            var squadInDb = await _squadRepository.GetAsync(userId);
            if (squad.Id != squadInDb.Id) return false;
            //TODO all ids must be unique
            //TODO Check cards belong to current user
            var allowedPositions = FormationLookup.AllowedPositions[squad.Formation].Select(x => x.ToString());
            var inLineup = squad.Lineup.Keys.Except(allowedPositions);
            var inAllowed = allowedPositions.Except(squad.Lineup.Keys);
            return !inLineup.Any() && !inAllowed.Any();
        }
    }
}
