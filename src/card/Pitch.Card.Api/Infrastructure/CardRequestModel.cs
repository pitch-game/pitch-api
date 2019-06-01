namespace Pitch.Card.Api.Models
{
    public class CardRequestModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string PositionPriority { get; set; }
    }
}
