﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Providers;

namespace Pitch.Match.Engine.Services
{
    public interface IFitnessDrainService
    {
        void Drain(Models.Match match, int minute);
    }

    public class FitnessDrainService : IFitnessDrainService
    {
        private readonly IRandomnessProvider _randomnessProvider;

        public FitnessDrainService(IRandomnessProvider randomnessProvider)
        {
            _randomnessProvider = randomnessProvider;
        }

        public void Drain(Models.Match match, int minute)
        {
           DrainLineup(match, minute, match.HomeTeam.Squad.Lineup);
           DrainLineup(match, minute, match.AwayTeam.Squad.Lineup);
        }

        private void DrainLineup(Models.Match match, int minute, IDictionary<string, IEnumerable<Card>> lineup)
        {
            var allCards = lineup.SelectMany(x => x.Value);
            foreach (var card in allCards.Where(x => x != null))
            {
                AddModifier(match, minute, card.Id, CalculateFitnessDrain());
            }
        }

        private float CalculateFitnessDrain()
        {
            return 0.01f * _randomnessProvider.Next(200); //TODO drain based on position
        }

        private void AddModifier(Models.Match match, int minute, Guid cardId, float value)
        {
            match.Minutes[minute].Modifiers.Add(new Modifier(cardId, value, ModifierType.Fitness));
        }
    }
}