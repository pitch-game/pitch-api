using Newtonsoft.Json;
using System;

namespace Pitch.Player.Api.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; }

        public string[] Positions
        {
            get //TODO just while the data isn't complete
            {
                if (Position == "CM")
                    return new string[] { "CM", "RM", "LM" };
                if (Position == "CB")
                    return new string[] { "CB", "RB", "LB" };
                return new string[] { Position };
            }
        }
    }
}
