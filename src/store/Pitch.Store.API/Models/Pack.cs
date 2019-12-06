using System;
using Pitch.Store.API.Infrastructure;

namespace Pitch.Store.API.Models
{
    public class Pack : IEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Position { get; set; }
    }
}
