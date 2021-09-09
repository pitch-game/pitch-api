using System;
using System.Collections.Generic;
using System.Linq;
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
            var stubMatch = new ApplicationCore.Models.Match.Match(){
                HomeTeam = new TeamDetails(){
                    Squad = new ApplicationCore.Models.Squad(){
                        Lineup = new Dictionary<string, IEnumerable<ApplicationCore.Models.Card>>()
                    }
                },
                AwayTeam = new TeamDetails(){
                    Squad = new ApplicationCore.Models.Squad(){
                        Lineup = new Dictionary<string, IEnumerable<ApplicationCore.Models.Card>>(){
                           {
                               "LST", new List<ApplicationCore.Models.Card>(){
                                    new ApplicationCore.Models.Card(){
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
