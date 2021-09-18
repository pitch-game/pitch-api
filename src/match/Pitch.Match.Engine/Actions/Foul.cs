using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Providers;

namespace Pitch.Match.Engine.Actions
{
    public class Foul : IAction
    {
        private readonly IRandomnessProvider _randomnessProvider;

        public Foul(IRandomnessProvider randomnessProvider)
        {
            this._randomnessProvider = randomnessProvider;
        }

        //[BsonIgnore]
        public decimal ChancePerMinute => 0.30m;

        //[BsonIgnore]
        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.05m },
            { PositionalArea.DEF, 0.40m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.15m },
        };

        //[BsonIgnore]
        public bool AffectsTeamInPossession => false;

        public Event SpawnEvent(Card card, Guid squadId, Models.Match match)
        {
            int randomNumber = _randomnessProvider.Next(1, 40);

            if (randomNumber == 1)
            {
                return new Event(EventType.RedCard, card.Id, squadId);
            }
            if (randomNumber >= 2 && randomNumber < 5)
            {
                var yellowCards = match.Minutes.SelectMany(x => x.Events).Where(x => x.Type == EventType.YellowCard).Select(x => x.CardId);
                if (yellowCards.Contains(card.Id))
                {
                    return new Event(EventType.RedCard, card.Id, squadId); //TODO return yellow and red event
                }
                return new Event(EventType.YellowCard, card.Id, squadId);
            }
            if (randomNumber >= 5)
                return new Event(EventType.Foul, card.Id, squadId);
            return null;
        }
    }
}
