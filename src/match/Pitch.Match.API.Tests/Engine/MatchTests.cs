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
            StubMatch.KickOff = DateTime.Now.AddMinutes(-10);
            var stubEvent = new ShotOnTarget(11, StubHomePlayer.Id, StubMatch.HomeTeam.Squad.Id);
            StubMatch.Minutes[11].Events.Add(stubEvent);
            var stubStatistic = new MinuteStats(11, StubHomeSquad.Id, 0, 0);
            StubMatch.Minutes[11].Stats = stubStatistic;

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.DoesNotContain(StubMatch.Events, x => x == stubEvent);
            Assert.DoesNotContain(StubMatch.Statistics, x => x == stubStatistic);
        }

        [Fact]
        public void AsOfNow_UpUntilCurrentMinute_IncludesEventsAndStatistics()
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-15);
            var stubEvent = new ShotOnTarget(11, StubHomePlayer.Id, StubMatch.HomeTeam.Squad.Id);
            StubMatch.Minutes[11].Events.Add(stubEvent);
            var stubStatistic = new MinuteStats(11, StubHomeSquad.Id, 0, 0);
            StubMatch.Minutes[11].Stats = stubStatistic;

            //Act
            StubMatch.AsAtElapsed();

            //Assert
            Assert.Contains(StubMatch.Events, x => x == stubEvent);
            Assert.Contains(StubMatch.Statistics, x => x == stubStatistic);
        }

        [Fact]
        public void GetSquad_ReturnsCorrectSquad()
        {
            var homeSquad = StubMatch.GetSquad(StubHomeSquad.Id);
            Assert.Equal(StubHomeSquad, homeSquad);
        }

        [Fact]
        public void GetOppositionSquad_ReturnsCorrectSquad()
        {
            var awaySquad = StubMatch.GetOppositionSquad(StubHomeSquad.Id);
            Assert.Equal(StubAwaySquad, awaySquad);
        }

        [Fact]
        public void GetTeam_ReturnsCorrectTeam()
        {
            var teamDetails = StubMatch.GetTeam(StubHomeUserId);
            Assert.Equal(StubHomeTeamDetails, teamDetails);
        }
    }
}