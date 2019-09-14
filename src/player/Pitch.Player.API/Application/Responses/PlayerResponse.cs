using System;

namespace Pitch.Player.API.Application.Responses
{
    public class PlayerResponse
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string[] Positions { get; set; }
        public int Rating { get; set; }
        public decimal Form { get; set; }
    }
}
