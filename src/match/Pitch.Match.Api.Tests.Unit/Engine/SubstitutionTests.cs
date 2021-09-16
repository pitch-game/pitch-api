using System;
using System.Linq;
using Pitch.Match.Engine.Events;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit.Engine
{
    public class SubstitutionTests : MatchTestBase
    {
        //Fact or theory? That is the question...
        [Theory]
        [InlineData(0)]
        [InlineData(67)]
        [InlineData(89)]
        public void Substitution_OnSameMinuteAsAnotherEvent_SubstitutionComesLast(int minute)
        {
            //Arrange
            StubMatch.KickOff = DateTime.Now.AddMinutes(-minute);
            StubMatch.Minutes[minute].Events.Add(new ShotOnTarget(StubHomePlayer.Id, StubMatch.HomeTeam.Squad.Id));

            //Act
            StubMatch.Substitute(StubHomePlayer.Id, StubHomeSub.Id, StubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            //Assert
            var shotEvent = StubMatch.Minutes[minute].Events.FirstOrDefault(x => x.CardId == StubHomePlayer.Id && x is ShotOnTarget);
            var subEvent = StubMatch.Minutes[minute].Events.FirstOrDefault(x => x.CardId == StubHomeSub.Id && x is Substitution);
            var shotEventIndex = StubMatch.Minutes[minute].Events.IndexOf(shotEvent);
            var subEventIndex = StubMatch.Minutes[minute].Events.IndexOf(subEvent);
            Assert.True(subEventIndex > shotEventIndex);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(67)]
        [InlineData(89)]
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
            Assert.Contains(StubMatch.Minutes[minute].Events, x => x.SquadId == StubMatch.HomeTeam.Squad.Id && x.CardId == StubHomeSub.Id && x is Substitution);
            Assert.Contains(StubMatch.Minutes[minute].Events, x => x.SquadId == StubMatch.AwayTeam.Squad.Id && x.CardId == StubAwaySub.Id && x is Substitution);
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