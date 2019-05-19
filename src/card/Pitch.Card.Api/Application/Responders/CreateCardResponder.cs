using AutoMapper;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure.Requests;
using Pitch.Card.Api.Infrastructure.Services;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Handlers
{
    public class CreateCardResponder : ICreateCardResponder
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public CreateCardResponder(ICardService cardService, IMapper mapper)
        {
            _cardService = cardService;
            _mapper = mapper;
        }

        public async Task<CreateCardResponse> Response(CreateCardRequest @request)
        {
            var card = await _cardService.CreateCardAsync();
            return _mapper.Map<CreateCardResponse>(card);
        }
    }
}
