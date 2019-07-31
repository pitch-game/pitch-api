using Pitch.Match.Api.Application.Engine.Events;
using System;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class MatchTests : MatchTestBase
    {
        //TODO Test reentrancy

        [Fact]
        public void AsOfNow_RemovesEventsAndStatisticsOnOrAfterThatMinute()
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubEvent = new ShotOnTarget(11, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id);
            _stubMatch.Events.Add(stubEvent);

            //Act
            _stubMatch.AsOfNow();

            //Assert
            Assert.DoesNotContain(_stubMatch.Events, x => x == stubEvent);
        }

        [Fact]
        public void AsOfNow_MaintainsEventsAndStatisticsBeforeThatMinute()
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-15);
            var stubEvent = new ShotOnTarget(11, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id);
            _stubMatch.Events.Add(stubEvent);

            //Act
            _stubMatch.AsOfNow();

            //Assert
            Assert.Contains(_stubMatch.Events, x => x == stubEvent);
        }

        //TODO can't currently test this due to implementation
        [Fact]
        public void TwoYellowCards_ForSamePlayer_SendsThemOff() { }

        //TODO can't currently test this due to implementation
        [Fact]
        public void RedCard_ForPlayer_SendsThemOff() { }
    }
}
