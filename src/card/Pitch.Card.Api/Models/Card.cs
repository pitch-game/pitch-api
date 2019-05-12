using System;

namespace Pitch.Card.Api.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public bool Opened { get; set; }
    }
}
