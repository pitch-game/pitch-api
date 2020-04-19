namespace Pitch.Match.API.ApplicationCore.Models.MatchResult {
    public class Lineup  {
        
        public Lineup(Squad home, Squad away)
        {
            Home = home;
            Away = away;
        }

        public Squad Home { get; set; }
        public Squad Away { get; set; }
    }
}