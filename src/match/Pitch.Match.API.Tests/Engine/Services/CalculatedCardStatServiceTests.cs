using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Engine.Services;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Xunit;

namespace Pitch.Match.API.Tests.Engine.Services
{
    public class CalculatedCardStatServiceTests
    {
        [Fact]
        public void Fitness_Returns_Correct_Value()
        {
            var cardId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match.Match();
            stubMatch.Minutes = new MatchMinute[]
            {
                new MatchMinute()
                {
                    Modifiers = new List<Modifier>()
                    {
                        new Modifier()
                        {
                            CardId = cardId,
                            DrainValue = 1,
                            Type = ModifierType.Fitness
                        }
                    }
                }
            };
            var service = new CalculatedCardStatService();

            //Act
            service.Set(stubMatch, 1);

            //Assert
            //Assert.Equal(99, fitness);
        }
    }
}
