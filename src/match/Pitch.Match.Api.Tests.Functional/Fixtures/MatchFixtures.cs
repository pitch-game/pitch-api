using System;
using Pitch.Match.Api.Tests.Shared;
using Pitch.Match.Api.Tests.Shared.Builders;
using Pitch.Match.Engine.Events;

namespace Pitch.Match.Api.Tests.Functional.Fixtures
{
    public class MatchFixtures
    {
        public MatchDtoBuilder DefaultMatch => new MatchDtoBuilder()
            .WithId(TestConstants.DefaultMatchId)
            .WithKickOff(DateTime.Now.AddMinutes(-34))
            .WithHomeTeam(new TeamDetailsBuilder()
                .WithUserId(TestConstants.DefaultUserId)
                .WithSquad(new SquadBuilder()
                    .WithId(TestConstants.DefaultHomeSquadId)
                    .WithName("Default FC")
                    .WithCardsInLineup("ST", new[]
                    {
                        new CardBuilder()
                            .WithId(TestConstants.DefaultHomeActiveCardId)
                            .WithName("Jimmy Johnson")
                    })
                    .WithSubs(new[]
                    {
                        new CardBuilder()
                            .WithId(TestConstants.DefaultHomeSubCardId)
                    })))
            .WithAwayTeam(new TeamDetailsBuilder()
                .WithSquad(new SquadBuilder()
                    .WithName("Evil FC")
                    .WithId(TestConstants.DefaultAwaySquadId)
                    .WithCardsInLineup("ST", new[]
                    {
                        new CardBuilder()
                            .WithId(TestConstants.DefaultAwayActiveCardId)
                    })
                    .WithSubs(new[]
                    {
                        new CardBuilder()
                            .WithId(TestConstants.DefaultAwaySubCardId)
                    })))
            .WithMinute(0, new MatchMinuteBuilder().WithMinuteStats(new MinuteStatsBuilder().WithSquadInPossession(TestConstants.DefaultHomeSquadId))
                .WithEvent(new ShotOnTarget(TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId)))
            .WithMinute(20, new MatchMinuteBuilder().WithMinuteStats(new MinuteStatsBuilder().WithSquadInPossession(TestConstants.DefaultHomeSquadId))
                .WithEvent(new Goal(TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId)));
    }
}