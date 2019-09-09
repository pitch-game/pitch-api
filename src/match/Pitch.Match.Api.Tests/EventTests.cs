using Pitch.Match.Api.ApplicationCore.Engine.Events;
using System;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class EventTests
    {
        [Fact]
        public void FoulEventHasCorrectName()
        {
            var foul = new Foul(0, new Guid(), new Guid());
            Assert.Equal("Foul", foul.Name);
        }

        [Fact]
        public void FoulEventsDoNotAppearInTimeline()
        {
            var foul = new Foul(0, new Guid(), new Guid());
            Assert.False(foul.ShowInTimeline);
        }

        [Fact]
        public void YellowCardEventHasCorrectName()
        {
            var yellowCard = new YellowCard(0, new Guid(), new Guid());
            Assert.Equal("Yellow Card", yellowCard.Name);
        }

        [Fact]
        public void YellowCardEventsAppearInTimeline()
        {
            var yellowCard = new YellowCard(0, new Guid(), new Guid());
            Assert.True(yellowCard.ShowInTimeline);
        }

        [Fact]
        public void SubstitutionEventHasCorrectName()
        {
            var sub = new Substitution(0, new Guid(), new Guid());
            Assert.Equal("Substitution", sub.Name);
        }

        [Fact]
        public void SubstitutionEventsAppearInTimeline()
        {
            var sub = new Substitution(0, new Guid(), new Guid());
            Assert.True(sub.ShowInTimeline);
        }

        [Fact]
        public void ShotOnTargetEventHasCorrectName()
        {
            var sot = new ShotOnTarget(0, new Guid(), new Guid());
            Assert.Equal("Shot On Target", sot.Name);
        }

        [Fact]
        public void ShotOnTargetEventsAppearInTimeline()
        {
            var sot = new ShotOnTarget(0, new Guid(), new Guid());
            Assert.True(sot.ShowInTimeline);
        }
    }
}
