namespace Pitch.Player.API.Models
{
    public class PlayerRequestModel
    {
        //TODO support nullable
        public (int? lower, int? upper)? RatingRange { get; set; }
        public string Position { get; set; }
    }
}
