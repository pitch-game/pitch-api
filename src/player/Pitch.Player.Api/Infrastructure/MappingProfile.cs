using AutoMapper;
using Pitch.Player.Api.Application.Requests;
using Pitch.Player.Api.Application.Responders;
using Pitch.Player.Api.Application.Responses;
using Pitch.Player.Api.Models;

namespace Pitch.Player.Api.Infrastructure
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
