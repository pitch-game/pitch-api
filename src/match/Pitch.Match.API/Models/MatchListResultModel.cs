using System;

namespace Pitch.Match.API.Models
{
    public class MatchListResultModel
    {
        public Guid Id { get; set; }
        public string Result { get; set; }
        public bool Claimed { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime KickOff { get; set; }
    }
}
