using System;

namespace Pitch.Match.Api.Infrastructure.Models
{
    public class Modifier
    {
        public Guid CardId { get; set; }
        public float DrainValue { get; set; }
        public ModifierType Type { get; set; }
    }
}
