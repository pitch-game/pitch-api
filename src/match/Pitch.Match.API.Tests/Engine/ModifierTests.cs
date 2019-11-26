using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;
using Xunit;

namespace Pitch.Match.API.Tests.Engine
{
    public class ModifierTests : MatchTestBase
    {
        [Fact]
        public void AsOfNow_OnOrAfterCurrentMinute_ExcludesModifiers()
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubModifier = new Modifier()
            {
                CardId = Guid.NewGuid(),
                Type = ModifierType.Fitness,
                DrainValue = 1
            };
            StubMatch.Modifiers[11] = new []{ stubModifier };

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.True(StubMatch.Modifiers.Length == 10);
        }

    }
}
