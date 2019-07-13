using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Squad.Api.Application.Requests;
using Pitch.Squad.Api.Application.Response;
using Pitch.Squad.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Squad.Api.Application.Responders
{
    public interface IGetSquadResponder
    {
        Task<GetSquadResponse> Response(GetSquadRequest @request);
    }
    public class GetSquadResponder : IGetSquadResponder, IResponder
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetSquadResponder(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
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

                return new GetSquadResponse()
                {
                    Id = squad.Id,
                    Name = squad.Name,
                    Lineup = lineup,
                    Subs = subs
                };
            }
        }
    }
}
