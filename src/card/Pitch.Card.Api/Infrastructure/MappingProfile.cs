using AutoMapper;
using Pitch.Card.Api.Application.Responses;

namespace Pitch.Card.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Card, CreateCardResponse>();
        }
    }
}
