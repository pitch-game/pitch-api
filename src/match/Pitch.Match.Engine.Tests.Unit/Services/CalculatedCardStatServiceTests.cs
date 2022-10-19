using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Services;
using Xunit;

namespace Pitch.Match.Engine.Tests.Unit.Services
{
    public class CalculatedCardStatServiceTests
    {
        [Fact]
        public void Fitness_Returns_Correct_Value()
        {
            var cardId = Guid.NewGuid();
            var stubMatch = new Match.Engine.Models.Match(){
                HomeTeam = new TeamDetails(){
                    Squad = new Squad(){
                        Lineup = new Dictionary<string, IEnumerable<Card>>()
                    }
                },
                AwayTeam = new TeamDetails(){
                    Squad = new Squad(){
                        Lineup = new Dictionary<string, IEnumerable<Card>>(){
                           {
                               "LST", new List<Card>(){
                                    new Card(){
                                        Id = cardId
                                    }
                                }
                           }
                        }
                    }
                }
            };
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
            service.Set(stubMatch);

            //Assert
            Assert.Equal(99, stubMatch.AwayTeam.Squad.Lineup["LST"].First().Fitness);
        }
    }
}
