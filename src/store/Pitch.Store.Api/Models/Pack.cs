using System;

namespace Pitch.Store.API.Models
{
    public class Pack
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Position { get; set; }
    }
}
