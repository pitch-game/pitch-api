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
            StubMatch.Modifiers[10] = new []{ stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Empty(StubMatch.Modifiers[11]);
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
            StubMatch.Modifiers[11] = new[] { stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Empty(StubMatch.Modifiers[11]);
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
            StubMatch.Modifiers[8] = new[] { stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Equal(StubMatch.Modifiers[8][0], stubModifier);
        }
    }
}
