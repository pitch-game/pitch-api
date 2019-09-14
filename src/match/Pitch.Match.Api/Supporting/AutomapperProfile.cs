using AutoMapper;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.Models;

namespace Pitch.Match.API.Supporting
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Lineup, LineupModel>();
            CreateMap<MatchListResult, MatchListResultModel>();
        }
    }
}
