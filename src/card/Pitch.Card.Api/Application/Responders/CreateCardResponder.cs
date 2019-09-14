using AutoMapper;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Card.API.Application.Requests;
using Pitch.Card.API.Application.Responses;
using Pitch.Card.API.Infrastructure.Services;
using Pitch.Card.API.Models;
using System.Threading.Tasks;

namespace Pitch.Card.API.Application.Responders
{
    public class CreateCardResponder : ICreateCardResponder, IResponder
    {
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CreateCardResponder(IMapper mapper, IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _mapper = mapper;
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Register()
        {
            _bus.RespondAsync<CreateCardRequest, CreateCardResponse>(Response);
        }

        public async Task<CreateCardResponse> Response(CreateCardRequest @request)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var reqModel = _mapper.Map<CreateCardModel>(@request);
                var card = await scope.ServiceProvider.GetRequiredService<ICardService>().CreateCardAsync(reqModel);
                return _mapper.Map<CreateCardResponse>(card);
            }
        }
    }
}
