using MongoDB.Bson.Serialization.Attributes;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Application.Engine.Helpers;
using Pitch.Match.Api.Models;
using System;
using System.Collections.Generic;


namespace Pitch.Match.Api.Application.Engine.Action
{
    public class Shot : IAction
    {
        private const double SHOOTER_AGAINST_DEFENDERS_MODIFIER = 0.8;
        private const double SHOOTER_AGAINST_GK_MODIFIER = 0.8;

        [BsonIgnore]
        public decimal ChancePerMinute => 0.2m;

        [BsonIgnore]
        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.0m },
            { PositionalArea.DEF, 0.10m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.50m },
        };

        [BsonIgnore]
        public bool AffectsTeamInPossession => true;

        public IEvent SpawnEvent(Card card, Guid squadId, int minute, Models.Match match, out bool forceReRoll)
        {
            forceReRoll = false;

            var oppositionsDefenceRating = RatingHelper.CurrentRating(PositionalArea.DEF, match.GetOppositionSquad(squadId));
            var shootersRating = RatingHelper.CurrentRating(card.Id, match.GetSquad(squadId));

            //var difference = Math.Abs(oppositionsDefenceRating - shootersRating);

            //var onTargetChance = (int)Math.Round((1 - ((double)difference / (double)shootersRating)) * 100);
            //var offTargetChance = (int)Math.Round((1 -((double)difference / (double)oppositionsDefenceRating)) * 100);

            var shotOnTargetChance = (int)Math.Round(oppositionsDefenceRating + (shootersRating * SHOOTER_AGAINST_DEFENDERS_MODIFIER));

            var rand = new Random();
            var randomNumber = rand.Next(0, shotOnTargetChance);
            if (randomNumber <= shootersRating * SHOOTER_AGAINST_DEFENDERS_MODIFIER)
            {
                var gkRating = RatingHelper.CurrentRating(PositionalArea.GK, match.GetOppositionSquad(squadId));
                //var shotDifference = Math.Abs(gkRating - shootersRating);

                //var goalChance = (int)Math.Round((1 - ((double)shotDifference / (double)shootersRating)) * 100);
                //var saveChance = (int)Math.Round((1 - ((double)shotDifference / (double)gkRating)) * 100);

                var goalChanceAccum = (int)Math.Round(gkRating + (shootersRating * SHOOTER_AGAINST_GK_MODIFIER));

                var goalRandomNumber = rand.Next(0, goalChanceAccum);
                if (goalRandomNumber <= shootersRating * SHOOTER_AGAINST_GK_MODIFIER)
                {
                    return new Goal(minute, card.Id, squadId);
                }
                return new ShotOnTarget(minute, card.Id, squadId);
            } else
            {
                return new ShotOffTarget(minute, card.Id, squadId);
            }
        }
    }
}
