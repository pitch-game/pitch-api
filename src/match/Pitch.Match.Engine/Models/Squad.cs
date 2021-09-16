using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Engine.Models
{
    public class Squad
    {
        public Squad()
        {
            Lineup = new Dictionary<string, IEnumerable<Card>>();
            Subs = new Card[6];
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IDictionary<string, IEnumerable<Card>> Lineup { get; set; }
        public Card[] Subs { get; set; }

        public void Substitute(Guid off, Guid on)
        {
            Card offCard = Lineup.Values.First(x => x.Any(y => y.Id == off)).First(); //TODO single doesnt work?
            Card onCard = Subs.Single(x => x.Id == on);

            foreach (var pos in Lineup.ToDictionary(x => x.Key, x => x.Value)) //TODO ?
            {
                var cards = pos.Value.ToList();
                var index = cards.FindIndex(x => x.Id == offCard.Id);
                if (index >= 0)
                {
                    cards[index] = onCard;
                    Lineup[pos.Key] = cards;
                }
            }

            var subs = Subs.ToList();
            var subIndex = subs.FindIndex(x => x.Id == onCard.Id);
            Subs[subIndex] = offCard;
        }
    }

    //[BsonIgnoreExtraElements]
    public class Card
    {
        public Card()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Rating { get; set; }
        public string Rarity { get; set; }
        public string Position { get; set; }
        public int Chemistry { get; set; }
        public int Fitness { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
    }
}
