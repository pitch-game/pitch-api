using System;

namespace Pitch.Squad.Api.Application.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        //TODO SPLIT FOR TWO DIFFERENT USE CASES

        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public int Fitness { get; set; }
        public string Position { get; set; } //TODO move out
    }
}
