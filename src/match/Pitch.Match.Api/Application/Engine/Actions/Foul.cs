using MongoDB.Bson.Serialization.Attributes;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Match.Api.Application.Engine.Action
{
    public class Foul : IAction
    {
        [BsonIgnore]
        public decimal ChancePerMinute => 0.04m;

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
            //TODO base on fitness and ratings
            Random rnd = new Random();
            int randomNumber = rnd.Next(1, 21);

            if (randomNumber == 1)
            {
                card.SentOff = true; //TODO doesn't support reetrancy
                return new RedCard(minute, card.Id, squadId);
            }
            if (randomNumber >= 2 && randomNumber < 7)
            {
                //todo check for two yellows
                return new YellowCard(minute, card.Id, squadId);
            }
            if (randomNumber >= 7)
                return null; //Just a foul
            return null;
        }
    }
}
