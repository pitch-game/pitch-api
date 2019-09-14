using System;

namespace Pitch.Card.API.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public bool Opened { get; set; }
        public decimal Form { get; set; }
        public int Fitness { get; set; }
        public int PreviousOwners { get; set; }
        public int GamesPlayed { get; set; }
        public int GoalsScored { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
