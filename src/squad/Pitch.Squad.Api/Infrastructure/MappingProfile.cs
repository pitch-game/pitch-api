using AutoMapper;
using Pitch.Squad.Api.Application.Models;
using Pitch.Squad.Api.Models;

namespace Pitch.Squad.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CardDTO, Card>();
        }
    }
}
