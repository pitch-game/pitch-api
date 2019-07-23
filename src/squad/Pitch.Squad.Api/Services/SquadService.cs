using AutoMapper;
using EasyNetQ;
using Pitch.Squad.Api.Application.Requests;
using Pitch.Squad.Api.Application.Response;
using Pitch.Squad.Api.Infrastructure.Repositories;
using Pitch.Squad.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Services
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
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public SquadService(ISquadRepository squadRepository, ISquadValidationService squadValidationService, IBus bus, IMapper mapper)
        {
            _squadRepository = squadRepository;
            _squadValidationService = squadValidationService;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<Models.Squad> GetOrCreateAsync(string userId)
        {
            var activeSquad = await _squadRepository.GetAsync(userId);
            //if(activeSquad != null)
            //{
            //    var cardIds = activeSquad.Lineup.Where(x => x.Value.HasValue).Select(x => x.Value).Cast<Guid>()
            //        .Concat(activeSquad.Subs.Where(x => x != null).Select(x => x.Value)).ToList(); //TODO move out
            //    var request = new GetCardsRequest(cardIds);
            //    var response = await _bus.RequestAsync<GetCardsRequest, GetCardsResponse>(request);

            //    activeSquad.Cards = _mapper.Map<IList<Card>>(response.Cards);
            //}
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
