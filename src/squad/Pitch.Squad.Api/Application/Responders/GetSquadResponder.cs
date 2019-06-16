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
                var cardsResponse = await _bus.RequestAsync<GetCardsRequest, GetCardsResponse>(new GetCardsRequest(squad.Lineup.Where(x => x.Value.HasValue).Select(x => x.Value.Value).ToList()));

                //set positions TODO refactor
                foreach (var position in squad.Lineup)
                {
                    var card = cardsResponse.Cards.FirstOrDefault(x => x.Id == position.Value);
                    card.Position = position.Key;
                }

                return new GetSquadResponse()
                {
                    Cards = cardsResponse.Cards.ToList()
                };
            }
        }
    }
}
