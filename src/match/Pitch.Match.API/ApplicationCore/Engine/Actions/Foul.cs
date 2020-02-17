using MongoDB.Bson.Serialization.Attributes;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Engine.Providers;
using Pitch.Match.API.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.API.ApplicationCore.Engine.Actions
{
    public class Foul : IAction
    {
        private readonly IRandomnessProvider _randomnessProvider;

        public Foul(IRandomnessProvider randomnessProvider)
        {
            this._randomnessProvider = randomnessProvider;
        }

        [BsonIgnore]
        public decimal ChancePerMinute => 0.30m;

        [BsonIgnore]
        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.05m },
            { PositionalArea.DEF, 0.40m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.15m },
        };

        [BsonIgnore]
        public bool AffectsTeamInPossession => false;

        public IEvent SpawnEvent(Card card, Guid squadId, Models.Match match)
        {
            int randomNumber = _randomnessProvider.Next(1, 40);

            if (randomNumber == 1)
            {
                return new RedCard(card.Id, squadId);
            }
            if (randomNumber >= 2 && randomNumber < 5)
            {
                var yellowCards = match.Minutes.SelectMany(x => x.Events).Where(x => x is YellowCard).Select(x => x.CardId);
                if (yellowCards.Contains(card.Id))
                {
                    return new RedCard(card.Id, squadId); //TODO return yellow and red event
                }
                return new YellowCard(card.Id, squadId);
            }
            if (randomNumber >= 5)
                return new Events.Foul(card.Id, squadId);
            return null;
        }
    }
}
