using Pitch.Match.Api.Application.Engine;
using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Application.Engine.Events;
using System;
using System.Linq;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class SubstitutionTests : MatchTestBase
    {
        [Fact]
        public void WhenPlayerIsSubstitutedOnSameMinuteAnEventOccursRelatedToHim()
        {
            var match = SetUpMatch();
            match.KickOff = DateTime.Now.AddMinutes(-58); //Skip to 58th minute

            var player = match.HomeTeam.Squad.Lineup.SelectMany(x => x.Value).First();
            var sub = match.HomeTeam.Squad.Subs[0];
            match.Events.Add(new ShotOnTarget(58, player.Id, match.HomeTeam.Squad.Id)); //Manually add event on 58th minute

            match.Substitute(player.Id, sub.Id, match.HomeTeam.UserId); //Substitute on 58th minute

            var actions = new IAction[] { new Application.Engine.Action.Foul(), new Shot() };
            var engine = new MatchEngine(actions);

            var result = engine.SimulateReentrant(match);

            var shotEvent = result.Events.FirstOrDefault(x => x.Minute == 58 && x.CardId == player.Id && x.GetType() == typeof(ShotOnTarget));
            var subEvent = result.Events.FirstOrDefault(x => x.Minute == 58 && x.CardId == sub.Id && x.GetType() == typeof(Substitution));

            var shotEventIndex = result.Events.IndexOf(shotEvent);
            var subEventIndex = result.Events.IndexOf(subEvent);

            //Always show the substitution last
            Assert.True(subEventIndex > shotEventIndex);
        }

        [Fact]
        public void WhenTwoSubstitutionsOccurOnSameMinute()
        {
            var match = SetUpMatch();
            match.KickOff = DateTime.Now.AddMinutes(-58); //Skip to 58th minute

            var player = match.HomeTeam.Squad.Lineup.SelectMany(x => x.Value).First();
            var sub = match.HomeTeam.Squad.Subs[0];

            match.Substitute(player.Id, sub.Id, match.HomeTeam.UserId); //Substitute on 58th minute

            var actions = new IAction[] { new Application.Engine.Action.Foul(), new Shot() };
            var engine = new MatchEngine(actions);

            engine.SimulateReentrant(match);

            var awayPlayer = match.AwayTeam.Squad.Lineup.SelectMany(x => x.Value).First();
            var awaySub = match.AwayTeam.Squad.Subs[0];

            match.Substitute(awayPlayer.Id, awaySub.Id, match.AwayTeam.UserId); //Substitute on 58th minute

            var result = engine.SimulateReentrant(match);

            Assert.Contains(result.Events, x => x.Minute == 58 && x.SquadId == match.HomeTeam.Squad.Id && x.CardId == sub.Id &&  x.GetType() == typeof(Substitution));
            Assert.Contains(result.Events, x => x.Minute == 58 && x.SquadId == match.AwayTeam.Squad.Id && x.CardId == awaySub.Id && x.GetType() == typeof(Substitution));
        }

        [Fact]
        public void WhenASubstitutionIsMade()
        {
            //Ensure players swap
            //Assert.False(true);
        }
    }
}
