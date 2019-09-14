using AutoMapper;
using Pitch.Card.API.Application.Requests;
using Pitch.Card.API.Application.Responders;
using Pitch.Card.API.Application.Responses;
using Pitch.Card.API.Models;

namespace Pitch.Card.API.Infrastructure
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
