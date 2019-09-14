using AutoMapper;
using Pitch.Squad.API.Application.Models;
using Pitch.Squad.API.Models;

namespace Pitch.Squad.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CardDTO, Card>();
        }
    }
}
