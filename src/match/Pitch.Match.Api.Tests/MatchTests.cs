using Pitch.Match.Api.Application.Engine.Events;
using System;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class MatchTests : MatchTestBase
    {
        //TODO Test reentrancy

        [Fact]
        public void AsOfNow_OnOrAfterCurrentMinute_ExcludesEventsAndStatistics()
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubEvent = new ShotOnTarget(11, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id);
            _stubMatch.Events.Add(stubEvent);
            var stubStatistic = new Models.MinuteStats(11, _stubHomeSquad.Id, 0, 0);
            _stubMatch.Statistics.Add(stubStatistic);

            //Act
            _stubMatch.AsOfNow();

            //Assert
            Assert.DoesNotContain(_stubMatch.Events, x => x == stubEvent);
            Assert.DoesNotContain(_stubMatch.Statistics, x => x == stubStatistic);
        }

        [Fact]
        public void AsOfNow_UpUntilCurrentMinute_IncludesEventsAndStatistics()
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-15);
            var stubEvent = new ShotOnTarget(11, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id);
            _stubMatch.Events.Add(stubEvent);
            var stubStatistic = new Models.MinuteStats(11, _stubHomeSquad.Id, 0, 0);
            _stubMatch.Statistics.Add(stubStatistic);

            //Act
            _stubMatch.AsOfNow();

            //Assert
            Assert.Contains(_stubMatch.Events, x => x == stubEvent);
            Assert.Contains(_stubMatch.Statistics, x => x == stubStatistic);
        }

        //TODO can't currently test this due to implementation
        [Fact]
        public void TwoYellowCards_ForSamePlayer_SendsThemOff() { }

        //TODO can't currently test this due to implementation
        [Fact]
        public void RedCard_ForPlayer_SendsThemOff() { }
    }
}
