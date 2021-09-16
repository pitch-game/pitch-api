using AutoMapper;
using Pitch.Match.Api.ApplicationCore.Models;
using Pitch.Match.Api.ApplicationCore.Models.MatchResult;
using Pitch.Match.Api.Models;
using Lineup = Pitch.Match.Api.ApplicationCore.Models.Lineup;

namespace Pitch.Match.Api.Supporting
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
