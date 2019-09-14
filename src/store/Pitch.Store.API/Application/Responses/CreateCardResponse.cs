using System;

namespace Pitch.Store.API.Application.Responses
{
    public class CreateCardResponse
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public bool Opened { get; set; }
        public decimal Form { get; set; }
    }
}
