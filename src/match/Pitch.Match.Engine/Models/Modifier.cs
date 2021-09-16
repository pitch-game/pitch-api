using System;

namespace Pitch.Match.Engine.Models
{
    public class Modifier
    {
        public Modifier()
        {

        }

        public Modifier(Guid cardId, float drainValue, ModifierType type)
        {
            CardId = cardId;
            DrainValue = drainValue;
            Type = type;
        }
        
        public Guid CardId { get; set; }
        public float DrainValue { get; set; }
        public ModifierType Type { get; set; }
    }
}