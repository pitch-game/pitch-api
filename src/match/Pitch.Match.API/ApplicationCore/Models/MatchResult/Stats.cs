namespace Pitch.Match.Api.ApplicationCore.Models.MatchResult
{
    public class Stats
    {
        public int Shots { get; set; }
        public int ShotsOnTarget { get; set; }
        public int Possession { get; set; }
        public int Fouls { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }
}