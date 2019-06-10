using AutoMapper;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Card.Api.Application.Requests;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure.Services;
using Pitch.Card.Api.Models;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Application.Responders
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
