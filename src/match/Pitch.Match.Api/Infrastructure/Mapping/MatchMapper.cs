namespace Pitch.Match.Api.Infrastructure.Mapping
{
    public static class MatchMapper
    {
        public static Models.Match Map(Engine.Models.Match match)
        {
            return new Models.Match() //TODO
            {
                Id = match.Id,
                AwayTeam = match.AwayTeam,
                HomeTeam = match.HomeTeam,
                KickOff = match.KickOff,
                Minutes = match.Minutes,
                Version = match.Version
            };
        }
    }
}
