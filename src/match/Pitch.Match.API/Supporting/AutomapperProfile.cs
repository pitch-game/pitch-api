using AutoMapper;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.MatchResult;
using Pitch.Match.API.Models;
using Lineup = Pitch.Match.API.ApplicationCore.Models.Lineup;

namespace Pitch.Match.API.Supporting
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Lineup, LineupModel>();
            CreateMap<MatchListResult, MatchListResultModel>();
            CreateMap<MatchResult, MatchResultModel>();
        }
    }
}
