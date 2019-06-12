using System;

namespace Pitch.Store.Api.Models
{
    public class Pack
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
    }

    public class PositionalPack : Pack
    {
        public string Position { get; set; }
    }
}
