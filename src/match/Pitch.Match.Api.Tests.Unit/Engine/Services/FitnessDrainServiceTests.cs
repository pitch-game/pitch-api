using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Services;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit.Engine.Services
{
    public class FitnessDrainServiceTests
    {
        [Fact]
        public void Drain_Adds_Modifier()
        {
            var cardId = Guid.NewGuid();
            var stubCard = new Card()
            {
                Id = cardId
            };
            var stubSquad = new Squad()
            {
                Lineup = new Dictionary<string, IEnumerable<Card>>()
                {
                    {"ST", new[] {stubCard}}
                }
            };
            var stubMatch = new Match.Engine.Models.Match()
            {
                HomeTeam = new TeamDetails()
                {
                    Squad = stubSquad
                },
                AwayTeam = new TeamDetails()
                {
                    Squad = new Squad()
                }
            };
            var minute = 50;

            var randomnessProvider = new ActionTests.TestRandomnessProvider(100);
            var service = new FitnessDrainService(randomnessProvider);

            //Act
            service.Drain(stubMatch, minute);

            //Assert
            Assert.NotNull(stubMatch.Minutes[minute].Modifiers.First());
            Assert.Equal(ModifierType.Fitness, stubMatch.Minutes[minute].Modifiers.First().Type);
            Assert.Equal(cardId, stubMatch.Minutes[minute].Modifiers.First().CardId);
            Assert.Equal(1, stubMatch.Minutes[minute].Modifiers.First().DrainValue);
        }
    }
}
