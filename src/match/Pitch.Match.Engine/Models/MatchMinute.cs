using System.Collections.Generic;

namespace Pitch.Match.Engine.Models
{
    public class MatchMinute
    {
        public MatchMinute()
        {
            Modifiers = new List<Modifier>();
            Events = new List<Event>();
        }

        public MinuteStats Stats { get; set; }
        public IList<Modifier> Modifiers { get; set; }
        public IList<Event> Events { get; set; }
    }
}