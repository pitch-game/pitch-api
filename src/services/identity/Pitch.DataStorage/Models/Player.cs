namespace Pitch.DataStorage.Models
{
    /// <summary>
    /// The real-world data related to a player
    /// </summary>
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public int Form { get; set; }
    }
}
