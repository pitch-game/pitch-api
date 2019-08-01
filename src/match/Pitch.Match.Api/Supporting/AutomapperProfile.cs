using AutoMapper;
using Pitch.Match.Api.ApplicationCore.Models.Match;
using Pitch.Match.Api.Models;

namespace Pitch.Match.Api.Supporting
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
