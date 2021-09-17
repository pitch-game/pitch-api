using System;

namespace Pitch.Match.Api.Infrastructure.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public string Position { get; set; }
        public int Chemistry { get; set; }
        public int Fitness { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }
}
