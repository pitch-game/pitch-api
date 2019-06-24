using MongoDB.Bson.Serialization.Attributes;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.Engine.Action
{
    public class Foul : IAction
    {
        [BsonIgnore]
        public decimal ChancePerMinute => 0.02m;

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

        public IEvent SpawnEvent(Card card, Guid squadId, int minute, Models.Match match)
        {
            //TODO chance of no card/yellow/red

            Random rnd = new Random();
            int randomNumber = rnd.Next(1, 3);

            if (randomNumber == 1)
                return new YellowCard(minute, card.Id, squadId);
            if (randomNumber == 2)
            {
                card.Rating = 0; //Reference?
                return new RedCard(minute, card.Id, squadId);
            }
            if (randomNumber == 3)
                return null; //No card
            return null;
        }
    }
}
