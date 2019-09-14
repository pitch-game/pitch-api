using AutoMapper;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Squad.API.Application.Requests;
using Pitch.Squad.API.Application.Response;
using Pitch.Squad.API.Models;
using Pitch.Squad.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.API.Application.Responders
{
    public interface IGetSquadResponder
    {
        Task<GetSquadResponse> Response(GetSquadRequest @request);
    }
    public class GetSquadResponder : IGetSquadResponder, IResponder
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public GetSquadResponder(IBus bus, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public void Register()
        {
            _bus.RespondAsync<GetSquadRequest, GetSquadResponse>(Response);
        }

        public async Task<GetSquadResponse> Response(GetSquadRequest @request)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var squad = await scope.ServiceProvider.GetRequiredService<ISquadService>().GetOrCreateAsync(@request.UserId.ToString()); //TODO Get only
                var cardIds = squad.Lineup.Where(x => x.Value.HasValue).Select(x => x.Value).Concat(squad.Subs.Where(x => x.HasValue)).Select(x => x.Value).ToList();
                var cardsResponse = await _bus.RequestAsync<GetCardsRequest, GetCardsResponse>(new GetCardsRequest(cardIds));

                var lineup = squad.Lineup.ToDictionary(x => x.Key, x => cardsResponse.Cards.FirstOrDefault(c => c.Id == x.Value));
                var subs = squad.Subs.Select(x => cardsResponse.Cards.FirstOrDefault(c => c.Id == x)).ToArray();

                var finalLineup = lineup.ToDictionary(x => x.Key, x => _mapper.Map<Card>(x.Value));

                scope.ServiceProvider.GetRequiredService<IChemistryService>().SetChemistry(finalLineup);

                return new GetSquadResponse()
                {
                    Id = squad.Id,
                    Name = squad.Name,
                    Lineup = finalLineup,
                    Subs = _mapper.Map<IEnumerable<Card>>(subs).ToArray()
                };
            }
        }
    }
}
