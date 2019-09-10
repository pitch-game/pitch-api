using MongoDB.Bson.Serialization.Attributes;
using Pitch.Match.Api.ApplicationCore.Engine.Events;
using Pitch.Match.Api.ApplicationCore.Engine.Providers;
using Pitch.Match.Api.ApplicationCore.Models.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.ApplicationCore.Engine.Actions
{
    public class Foul : IAction
    {
        private readonly IRandomnessProvider randomnessProvider;

        public Foul(IRandomnessProvider randomnessProvider)
        {
            this.randomnessProvider = randomnessProvider;
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

        public IEvent SpawnEvent(Card card, Guid squadId, int minute, Models.Match.Match match)
        {
            int randomNumber = randomnessProvider.Next(1, 40);

            if (randomNumber == 1)
            {
                return new RedCard(minute, card.Id, squadId);
            }
            if (randomNumber >= 2 && randomNumber < 5)
            {
                var yellowCards = match.Events.Where(x => x.GetType() == typeof(YellowCard)).Select(x => x.CardId);
                if (yellowCards.Contains(card.Id))
                {
                    return new RedCard(minute, card.Id, squadId); //TODO return yellow and red event
                }
                return new YellowCard(minute, card.Id, squadId);
            }
            if (randomNumber >= 5)
                return new Events.Foul(minute, card.Id, squadId);
            return null;
        }
    }
}
