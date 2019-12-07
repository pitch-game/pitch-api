using System;
using Pitch.Match.API.ApplicationCore.Models;
using Xunit;

namespace Pitch.Match.API.Tests.Engine
{
    public class ModifierTests : MatchTestBase
    {
        [Fact]
        public void AsOfNow_OnCurrentMinute_ExcludesModifiers()
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubModifier = new Modifier()
            {
                CardId = Guid.NewGuid(),
                Type = ModifierType.Fitness,
                DrainValue = 1
            };
            StubMatch.Minutes[10].Modifiers = new []{ stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Empty(StubMatch.Minutes[11].Modifiers);
        }

        [Fact]
        public void AsOfNow_AfterCurrentMinute_ExcludesModifiers()
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubModifier = new Modifier()
            {
                CardId = Guid.NewGuid(),
                Type = ModifierType.Fitness,
                DrainValue = 1
            };
            StubMatch.Minutes[11].Modifiers = new[] { stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Empty(StubMatch.Minutes[11].Modifiers);
        }

        [Fact]
        public void AsOfNow_BeforeCurrentMinute_IncludesModifiers()
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubModifier = new Modifier()
            {
                CardId = Guid.NewGuid(),
                Type = ModifierType.Fitness,
                DrainValue = 1
            };
            StubMatch.Minutes[8].Modifiers = new[] { stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Equal(StubMatch.Minutes[8].Modifiers[0], stubModifier);
        }
    }
}
