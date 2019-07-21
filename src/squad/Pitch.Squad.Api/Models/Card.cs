using System;

namespace Pitch.Squad.Api.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public int Fitness { get; set; }
        public int Chemistry { get; set; }
    }
}
