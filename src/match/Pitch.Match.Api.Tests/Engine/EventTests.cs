using System;
using Pitch.Match.Api.ApplicationCore.Engine.Events;
using Xunit;

namespace Pitch.Match.Api.Tests.Engine
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
        public void GoalEventAppearsInTimeline()
        {
            var goal = new Goal(0, new Guid(), new Guid());
            Assert.True(goal.ShowInTimeline);
        }

        [Fact]
        public void GoalEventHasCorrectName()
        {
            var goal = new Goal(0, new Guid(), new Guid());
            Assert.Equal("Goal", goal.Name);
        }

        [Fact]
        public void RedCardEventHasCorrectName()
        {
            var redCard = new RedCard(0, new Guid(), new Guid());
            Assert.Equal("Red Card", redCard.Name);
        }

        [Fact]
        public void RedCardEventsAppearInTimeline()
        {
            var redCard = new RedCard(0, new Guid(), new Guid());
            Assert.True(redCard.ShowInTimeline);
        }

        [Fact]
        public void ShotOffTargetEventHasCorrectName()
        {
            var sot = new ShotOffTarget(0, new Guid(), new Guid());
            Assert.Equal("Shot Off Target", sot.Name);
        }

        [Fact]
        public void ShotOffTargetEventsAppearInTimeline()
        {
            var sot = new ShotOffTarget(0, new Guid(), new Guid());
            Assert.True(sot.ShowInTimeline);
        }

        [Fact]
        public void ShotOnTargetEventHasCorrectName()
        {
            var sot = new ShotOnTarget(0, new Guid(), new Guid());
            Assert.Equal("Shot Saved", sot.Name);
        }

        [Fact]
        public void ShotOnTargetEventsAppearInTimeline()
        {
            var sot = new ShotOnTarget(0, new Guid(), new Guid());
            Assert.True(sot.ShowInTimeline);
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
    }
}