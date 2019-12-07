using System;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Xunit;

namespace Pitch.Match.API.Tests.Engine
{
    public class SubstitutionTests : MatchTestBase
    {
        //Fact or theory? That is the question...
        [Theory]
        [InlineData(0)]
        [InlineData(67)]
        [InlineData(90)]
        public void Substitution_OnSameMinuteAsAnotherEvent_SubstitutionComesLast(int minute)
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-minute);
            StubMatch.Minutes[minute].Events.Add(new ShotOnTarget(minute, StubHomePlayer.Id, StubMatch.HomeTeam.Squad.Id));

            //Act
            StubMatch.Substitute(StubHomePlayer.Id, StubHomeSub.Id, StubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            //Assert
            var shotEvent = StubMatch.Events.FirstOrDefault(x =>
                x.Minute == minute && x.CardId == StubHomePlayer.Id && x.GetType() == typeof(ShotOnTarget));
            var subEvent = StubMatch.Events.FirstOrDefault(x =>
                x.Minute == minute && x.CardId == StubHomeSub.Id && x.GetType() == typeof(Substitution));
            var shotEventIndex = StubMatch.Minutes[minute].Events.IndexOf(shotEvent);
            var subEventIndex = StubMatch.Minutes[minute].Events.IndexOf(subEvent);
            Assert.True(subEventIndex > shotEventIndex);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(67)]
        [InlineData(90)]
        public void Substitutions_OnSameMinute_AllOccur(int minute)
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-minute);

            //Act
            StubMatch.Substitute(StubHomePlayer.Id, StubHomeSub.Id, StubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            StubMatch.Substitute(StubAwayPlayer.Id, StubAwaySub.Id, StubMatch.AwayTeam.UserId);
            SimulateStubMatch();

            //Assert
            Assert.Contains(StubMatch.Events,
                x => x.Minute == minute && x.SquadId == StubMatch.HomeTeam.Squad.Id && x.CardId == StubHomeSub.Id &&
                     x.GetType() == typeof(Substitution));
            Assert.Contains(StubMatch.Events,
                x => x.Minute == minute && x.SquadId == StubMatch.AwayTeam.Squad.Id && x.CardId == StubAwaySub.Id &&
                     x.GetType() == typeof(Substitution));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(29)]
        [InlineData(30)]
        public void Substitution_PlayersAreSwapped(int minute)
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-minute); //Skip to minute

            //Act
            StubMatch.Substitute(StubHomePlayer.Id, StubHomeSub.Id, StubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            //Assert
            Assert.DoesNotContain(StubHomeSquad.Lineup.SelectMany(x => x.Value), x => x.Id == StubHomePlayer.Id);
            Assert.DoesNotContain(StubHomeSquad.Subs, x => x.Id == StubHomeSub.Id);
            Assert.Contains(StubHomeSquad.Lineup.SelectMany(x => x.Value), x => x.Id == StubHomeSub.Id);
            Assert.Contains(StubHomeSquad.Subs, x => x.Id == StubHomePlayer.Id);
        }
    }
}