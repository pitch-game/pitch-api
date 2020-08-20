using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Pitch.Squad.API.Models;
using Pitch.Squad.API.Services;
using Xunit;

namespace Pitch.Squad.API.Tests.Services
{
    public class ChemistryServiceTests
    {
        [Theory]

        [InlineData("LST", "ST", 100)]
        [InlineData("LST", "LM", 75)]
        [InlineData("LST", "RM", 75)]
        [InlineData("LST", "CM", 50)]
        [InlineData("LST", "LB", 0)]
        [InlineData("LST", "RB", 0)]
        [InlineData("LST", "CB", 0)]
        [InlineData("LST", "GK", 0)]

        [InlineData("RST", "ST", 100)]
        [InlineData("RST", "LM", 75)]
        [InlineData("RST", "RM", 75)]
        [InlineData("RST", "CM", 50)]
        [InlineData("RST", "LB", 0)]
        [InlineData("RST", "RB", 0)]
        [InlineData("RST", "CB", 0)]
        [InlineData("RST", "GK", 0)]
        public void Set_Expected_Chemistry(string lineupPosition, string cardPosition, int expectedChemistry)
        {
            var lineup = new Dictionary<string, Card>
            {
                {
                    lineupPosition,
                    new Card
                    {
                        Position = cardPosition
                    }
                }
            };
            var chemistryService = new ChemistryService();
            chemistryService.SetChemistryOnLineup(lineup);

            lineup.Values.First().Chemistry.Should().Be(expectedChemistry);
        }
    }
}