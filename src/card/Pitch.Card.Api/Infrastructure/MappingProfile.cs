using AutoMapper;
using Pitch.Card.Api.Application.Requests;
using Pitch.Card.Api.Application.Responders;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Models;

namespace Pitch.Card.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Card, CreateCardResponse>();
            CreateMap<CreateCardRequest, CreateCardModel>();
            CreateMap<CreateCardModel, PlayerRequest>();
        }
    }
}
