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

        public async Task<Models.Squad> Update(Models.Squad squad)
        {
            if (!ValidateSquad(squad)) throw new System.Exception("Squad formation is invalid.");
            return await _squadRepository.UpdateAsync(squad);
        }

        private bool ValidateSquad(Models.Squad squad) //TODO move to validation service
        {
            var allowedPositions = FormationLookup.AllowedPositions[squad.Formation];
            var inLineup = squad.Lineup.Keys.Except(allowedPositions);
            var inAllowed = allowedPositions.Except(squad.Lineup.Keys);
            return !inLineup.Any() && !inAllowed.Any();
        }
    }
}
