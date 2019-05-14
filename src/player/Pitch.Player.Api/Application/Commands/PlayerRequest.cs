namespace Pitch.Player.Consumer.Messages
{
    public class PlayerRequest
    {
        public PlayerRequest((int? lower, int? upper) ratingRange, string position = null)
        {
            RatingRange = ratingRange;
            Position = position;
        }

        //E.g. 50 - 99
        public (int? lower, int? upper) RatingRange { get; private set; }
        public string Position { get; private set; }
    }
}
