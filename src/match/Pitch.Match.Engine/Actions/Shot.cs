﻿using System;
using System.Collections.Generic;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Providers;
using Pitch.Match.Engine.Services;

namespace Pitch.Match.Engine.Actions
{
    public class Shot : IAction
    {
        private readonly IRandomnessProvider _randomnessProvider;
        private readonly IRatingService _ratingService;

        public Shot(IRandomnessProvider randomnessProvider, IRatingService ratingService)
        {
            _randomnessProvider = randomnessProvider;
            _ratingService = ratingService;
        }

        //[BsonIgnore]
        public decimal ChancePerMinute => 0.35m;

        //[BsonIgnore]
        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.0m },
            { PositionalArea.DEF, 0.10m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.50m },
        };

        //[BsonIgnore]
        public bool AffectsTeamInPossession => true;

        public Event SpawnEvent(Card card, Guid squadId, Models.Match match)
        {
            var oppositionsDefenceRating = _ratingService.CurrentRating(PositionalArea.DEF, match, match.GetOppositionSquad(squadId));
            var shootersRating = _ratingService.CurrentRating(card.Id, match, match.GetSquad(squadId));

            var shotOnTargetChance = (int)Math.Round(oppositionsDefenceRating + shootersRating * Constants.ShooterAgainstDefendersModifier);

            var randomNumber = _randomnessProvider.Next(0, shotOnTargetChance);
            if (randomNumber <= shootersRating * Constants.ShooterAgainstDefendersModifier)
            {
                var gkRating = _ratingService.CurrentRating(PositionalArea.GK, match, match.GetOppositionSquad(squadId));

                var goalChanceAccum = (int)Math.Round(gkRating + shootersRating * Constants.ShooterAgainstGkModifier);

                var goalRandomNumber = _randomnessProvider.Next(0, goalChanceAccum);
                if (goalRandomNumber <= shootersRating * Constants.ShooterAgainstGkModifier)
                {
                    return new Event(EventType.Goal, card.Id, squadId);
                }
                return new Event(EventType.ShotOnTarget, card.Id, squadId);
            }

            return new Event(EventType.ShotOffTarget, card.Id, squadId);
        }
    }
}
