using System;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;
using Xunit;

namespace Pitch.Match.API.Tests.Engine
{
    public class MatchTests : MatchTestBase
    {
        [Fact]
        public void AsOfNow_OnOrAfterCurrentMinute_ExcludesEventsAndStatistics()
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubEvent = new ShotOnTarget(11, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id);
            _stubMatch.Events.Add(stubEvent);
            var stubStatistic = new MinuteStats(11, _stubHomeSquad.Id, 0, 0);
            _stubMatch.Statistics.Add(stubStatistic);

            //Act
            _stubMatch.AsAtElapsed();

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
            var stubStatistic = new MinuteStats(11, _stubHomeSquad.Id, 0, 0);
            _stubMatch.Statistics.Add(stubStatistic);

            //Act
            _stubMatch.AsAtElapsed();

            //Assert
            Assert.Contains(_stubMatch.Events, x => x == stubEvent);
            Assert.Contains(_stubMatch.Statistics, x => x == stubStatistic);
        }

        [Fact]
        public void GetSquad_ReturnsCorrectSquad()
        {
            var homeSquad = _stubMatch.GetSquad(_stubHomeSquad.Id);
            Assert.Equal(_stubHomeSquad, homeSquad);
        }

        [Fact]
        public void GetOppositionSquad_ReturnsCorrectSquad()
        {
            var awaySquad = _stubMatch.GetOppositionSquad(_stubHomeSquad.Id);
            Assert.Equal(_stubAwaySquad, awaySquad);
        }

        [Fact]
        public void GetTeam_ReturnsCorrectTeam()
        {
            var teamDetails = _stubMatch.GetTeam(_stubHomeUserId);
            Assert.Equal(_stubHomeTeamDetails, teamDetails);
        }
    }
}