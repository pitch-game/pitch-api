using AutoMapper;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.Models;

namespace Pitch.Match.API.Supporting
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Lineup, LineupModel>();
            CreateMap<MatchListResult, MatchListResultModel>();
        }
    }
}
