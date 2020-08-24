using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Pitch.Squad.API.Exceptions;
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

        [InlineData("LM", "ST", 0)]
        [InlineData("LM", "LM", 100)]
        [InlineData("LM", "RM", 90)]
        [InlineData("LM", "CM", 75)]
        [InlineData("LM", "LB", 50)]
        [InlineData("LM", "RB", 50)]
        [InlineData("LM", "CB", 0)]
        [InlineData("LM", "GK", 0)]

        [InlineData("LCM", "ST", 0)]
        [InlineData("LCM", "LM", 75)]
        [InlineData("LCM", "RM", 75)]
        [InlineData("LCM", "CM", 100)]
        [InlineData("LCM", "LB", 0)]
        [InlineData("LCM", "RB", 0)]
        [InlineData("LCM", "CB", 0)]
        [InlineData("LCM", "GK", 0)]

        [InlineData("CM", "ST", 0)]
        [InlineData("CM", "LM", 75)]
        [InlineData("CM", "RM", 75)]
        [InlineData("CM", "CM", 100)]
        [InlineData("CM", "LB", 0)]
        [InlineData("CM", "RB", 0)]
        [InlineData("CM", "CB", 0)]
        [InlineData("CM", "GK", 0)]

        [InlineData("RCM", "ST", 0)]
        [InlineData("RCM", "LM", 75)]
        [InlineData("RCM", "RM", 75)]
        [InlineData("RCM", "CM", 100)]
        [InlineData("RCM", "LB", 0)]
        [InlineData("RCM", "RB", 0)]
        [InlineData("RCM", "CB", 0)]
        [InlineData("RCM", "GK", 0)]

        [InlineData("RM", "ST", 0)]
        [InlineData("RM", "LM", 90)]
        [InlineData("RM", "RM", 100)]
        [InlineData("RM", "CM", 75)]
        [InlineData("RM", "LB", 50)]
        [InlineData("RM", "RB", 50)]
        [InlineData("RM", "CB", 0)]
        [InlineData("RM", "GK", 0)]

        [InlineData("LB", "ST", 0)]
        [InlineData("LB", "LM", 50)]
        [InlineData("LB", "RM", 50)]
        [InlineData("LB", "CM", 0)]
        [InlineData("LB", "LB", 100)]
        [InlineData("LB", "RB", 90)]
        [InlineData("LB", "CB", 75)]
        [InlineData("LB", "GK", 0)]

        [InlineData("LCB", "ST", 0)]
        [InlineData("LCB", "LM", 0)]
        [InlineData("LCB", "RM", 0)]
        [InlineData("LCB", "CM", 0)]
        [InlineData("LCB", "LB", 75)]
        [InlineData("LCB", "RB", 75)]
        [InlineData("LCB", "CB", 100)]
        [InlineData("LCB", "GK", 0)]

        [InlineData("RCB", "ST", 0)]
        [InlineData("RCB", "LM", 0)]
        [InlineData("RCB", "RM", 0)]
        [InlineData("RCB", "CM", 0)]
        [InlineData("RCB", "LB", 75)]
        [InlineData("RCB", "RB", 75)]
        [InlineData("RCB", "CB", 100)]
        [InlineData("RCB", "GK", 0)]

        [InlineData("RB", "ST", 0)]
        [InlineData("RB", "LM", 50)]
        [InlineData("RB", "RM", 50)]
        [InlineData("RB", "CM", 0)]
        [InlineData("RB", "LB", 90)]
        [InlineData("RB", "RB", 100)]
        [InlineData("RB", "CB", 75)]
        [InlineData("RB", "GK", 0)]

        [InlineData("GK", "GK", 100)]
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

        [Fact]
        public void Invalid_Position_Throws_ChemistryException()
        {
            var lineup = new Dictionary<string, Card>
            {
                {
                    "INV",
                    new Card
                    {
                        Position = "CB"
                    }
                    }
                };


            var chemistryService = new ChemistryService();
            Action act = () => chemistryService.SetChemistryOnLineup(lineup);
            act.Should().Throw<ChemistryException>();
        }

        [Fact]
        public void Invalid_Card_Position_Returns_0()
        {
            var lineup = new Dictionary<string, Card>
            {
                {
                    "CB",
                    new Card
                    {
                        Position = "INV"
                    }
                }
            };


            var chemistryService = new ChemistryService();
            chemistryService.SetChemistryOnLineup(lineup);

            lineup.Values.First().Chemistry.Should().Be(0);
        }

        [Fact]
        public void Null_Card_Is_Skipped()
        {
            var lineup = new Dictionary<string, Card>
            {
                {
                    "CB",
                    null
                }
            };

            var chemistryService = new ChemistryService();
            chemistryService.SetChemistryOnLineup(lineup);

            lineup.Should().BeEquivalentTo(new Dictionary<string, Card>
            {
                {
                    "CB",
                    null
                }
            });
        }
    }
}