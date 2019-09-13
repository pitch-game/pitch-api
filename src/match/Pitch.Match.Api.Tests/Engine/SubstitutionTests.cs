using System;
using System.Linq;
using Pitch.Match.Api.ApplicationCore.Engine.Events;
using Xunit;

namespace Pitch.Match.Api.Tests.Engine
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
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-minute);
            _stubMatch.Events.Add(new ShotOnTarget(minute, _stubHomePlayer.Id, _stubMatch.HomeTeam.Squad.Id));

            //Act
            _stubMatch.Substitute(_stubHomePlayer.Id, _stubHomeSub.Id, _stubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            //Assert
            var shotEvent = _stubMatch.Events.FirstOrDefault(x =>
                x.Minute == minute && x.CardId == _stubHomePlayer.Id && x.GetType() == typeof(ShotOnTarget));
            var subEvent = _stubMatch.Events.FirstOrDefault(x =>
                x.Minute == minute && x.CardId == _stubHomeSub.Id && x.GetType() == typeof(Substitution));
            var shotEventIndex = _stubMatch.Events.IndexOf(shotEvent);
            var subEventIndex = _stubMatch.Events.IndexOf(subEvent);
            Assert.True(subEventIndex > shotEventIndex);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(67)]
        [InlineData(90)]
        public void Substitutions_OnSameMinute_AllOccur(int minute)
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-minute);

            //Act
            _stubMatch.Substitute(_stubHomePlayer.Id, _stubHomeSub.Id, _stubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            _stubMatch.Substitute(_stubAwayPlayer.Id, _stubAwaySub.Id, _stubMatch.AwayTeam.UserId);
            SimulateStubMatch();

            //Assert
            Assert.Contains(_stubMatch.Events,
                x => x.Minute == minute && x.SquadId == _stubMatch.HomeTeam.Squad.Id && x.CardId == _stubHomeSub.Id &&
                     x.GetType() == typeof(Substitution));
            Assert.Contains(_stubMatch.Events,
                x => x.Minute == minute && x.SquadId == _stubMatch.AwayTeam.Squad.Id && x.CardId == _stubAwaySub.Id &&
                     x.GetType() == typeof(Substitution));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(29)]
        [InlineData(30)]
        public void Substitution_PlayersAreSwapped(int minute)
        {
            //Arrange
            _stubMatch.KickOff = DateTime.Now.AddMinutes(-minute); //Skip to minute

            //Act
            _stubMatch.Substitute(_stubHomePlayer.Id, _stubHomeSub.Id, _stubMatch.HomeTeam.UserId);
            SimulateStubMatch();

            //Assert
            Assert.DoesNotContain(_stubHomeSquad.Lineup.SelectMany(x => x.Value), x => x.Id == _stubHomePlayer.Id);
            Assert.DoesNotContain(_stubHomeSquad.Subs, x => x.Id == _stubHomeSub.Id);
            Assert.Contains(_stubHomeSquad.Lineup.SelectMany(x => x.Value), x => x.Id == _stubHomeSub.Id);
            Assert.Contains(_stubHomeSquad.Subs, x => x.Id == _stubHomePlayer.Id);
        }
    }
}