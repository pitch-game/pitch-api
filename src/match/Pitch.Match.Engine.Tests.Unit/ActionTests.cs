using System;
using Pitch.Match.Engine.Models;
using Pitch.Match.Engine.Providers;
using Xunit;
using Foul = Pitch.Match.Engine.Actions.Foul;

namespace Pitch.Match.Engine.Tests.Unit
{
    public class ActionTests
    {
        public class TestRandomnessProvider : IRandomnessProvider
        {
            private readonly int _value;

            public TestRandomnessProvider(int value)
            {
                this._value = value;
            }

            public int Next(int maxValue)
            {
                return _value;
            }

            public int Next(int minValue, int maxValue)
            {
                return _value;
            }
        }

        [Fact]
        public void FoulAction_CanSpawn_FoulEvent()
        {
            //Arrange
            var randomnessProvider = new TestRandomnessProvider(6);
            var foul = new Foul(randomnessProvider);
            var card = new Card {Id = Guid.NewGuid()};

            //Act
            var @event = foul.SpawnEvent(card, new Guid(), new Match.Engine.Models.Match());

            //Assert
            Assert.Equal(EventType.Foul, @event.Type);
        }

        [Fact]
        public void FoulAction_CanSpawn_RedCardEvent()
        {
            //Arrange
            var randomnessProvider = new TestRandomnessProvider(1);
            var foul = new Foul(randomnessProvider);
            var card = new Card {Id = Guid.NewGuid()};

            //Act
            var @event = foul.SpawnEvent(card, new Guid(), new Match.Engine.Models.Match());

            //Assert
            Assert.Equal(EventType.RedCard, @event.Type);
        }

        [Fact]
        public void FoulAction_CanSpawn_YellowCardEvent()
        {
            //Arrange
            var randomnessProvider = new TestRandomnessProvider(3);
            var foul = new Foul(randomnessProvider);
            var card = new Card {Id = Guid.NewGuid()};

            //Act
            var @event = foul.SpawnEvent(card, new Guid(), new Match.Engine.Models.Match());

            //Assert
            Assert.Equal(EventType.YellowCard, @event.Type);
        }

        [Fact]
        public void FoulAction_OnAYellowCard_WithASecondYellow_ShouldSpawnARedCardEvent()
        {
            //Arrange
            var randomnessProvider = new TestRandomnessProvider(3);
            var foul = new Foul(randomnessProvider);
            var cardId = Guid.NewGuid();
            var card = new Card {Id = cardId};
            var match = new Match.Engine.Models.Match();
            match.Minutes[5].Events.Add(new Event(EventType.YellowCard, cardId, new Guid()));

            //Act
            var @event = foul.SpawnEvent(card, new Guid(), match);

            //Assert
            Assert.Equal(EventType.RedCard, @event.Type);
        }

        [Fact]
        public void FoulAction_WithInvalidRandomNumber_ShouldSpawnNullEvent()
        {
            //Arrange
            var randomnessProvider = new TestRandomnessProvider(-1);
            var foul = new Foul(randomnessProvider);
            var card = new Card {Id = Guid.NewGuid()};

            //Act
            var @event = foul.SpawnEvent(card, new Guid(), new Match.Engine.Models.Match());

            //Assert
            Assert.Null(@event);
        }
    }
}