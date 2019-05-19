using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Application.Responses
{
    public class PlayerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string[] Positions { get; set; }
        public int Rating { get; set; }
        public decimal Form { get; set; }
    }
}
