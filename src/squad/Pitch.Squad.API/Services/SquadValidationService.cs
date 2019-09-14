using Pitch.Squad.API.Application.Requests;
using Pitch.Squad.API.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;

namespace Pitch.Squad.API.Services
{
    public interface ISquadValidationService
    {
        Task<bool> Validate(Models.Squad squad, Guid squadId, string userId);
    }
    public class SquadValidationService : ISquadValidationService
    {
        private readonly IBus _bus;
        public SquadValidationService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<bool> Validate(Models.Squad squad, Guid usersSquadId, string userId)
        {
            return SquadIdCheck(squad, usersSquadId) && await CardCheck(squad, userId) && FormationCheck(squad) && DistinctCheck(squad);
        }

        private static bool SquadIdCheck(Models.Squad squad, Guid usersSquadId)
        {
            return squad.Id == usersSquadId;
        }

        private static bool DistinctCheck(Models.Squad squad)
        {
            return !AllCardIds(squad).GroupBy(x => x).Any(c => c.Count() > 1);
        }

        private async Task<bool> CardCheck(Models.Squad squad, string userId)
        {
            var cardIds = AllCardIds(squad);
            var request = new GetCardsRequest(cardIds);
            var response = await _bus.RequestAsync<GetCardsRequest, GetCardsResponse>(request);

            foreach (var cardId in cardIds)
            {
                var card = response.Cards.FirstOrDefault(x => x.Id == cardId);
                if (card == null || card.UserId != userId) return false;
            }
            return true;
        }

        private static bool FormationCheck(Models.Squad squad)
        {
            var allowedPositions = Models.FormationLookup.AllowedPositions[squad.Formation].Select(x => x.ToString());
            var inLineup = squad.Lineup.Keys.Except(allowedPositions);
            var inAllowed = allowedPositions.Except(squad.Lineup.Keys);

            return !inLineup.Any() && !inAllowed.Any();
        }

        private static IList<Guid> AllCardIds(Models.Squad squad)
        {
            return squad.Lineup.Select(x => x.Value).Concat(squad.Subs).Where(x => x.HasValue).Select(x => x.Value).ToList();
        }
    }
}
