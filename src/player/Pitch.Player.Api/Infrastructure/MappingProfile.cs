using AutoMapper;
using Pitch.Player.API.Application.Requests;
using Pitch.Player.API.Application.Responders;
using Pitch.Player.API.Application.Responses;
using Pitch.Player.API.Models;

namespace Pitch.Player.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PlayerRequest, PlayerRequestModel>();
            CreateMap<Models.Player, PlayerResponse>();
        }
    }
}
