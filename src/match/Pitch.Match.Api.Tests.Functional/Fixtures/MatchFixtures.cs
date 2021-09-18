using System;
using Pitch.Match.Api.Infrastructure.Models;
using Pitch.Match.Api.Tests.Shared;
using Pitch.Match.Api.Tests.Shared.Builders;

namespace Pitch.Match.Api.Tests.Functional.Fixtures
{
    public class MatchFixtures
    {
        public MatchDtoBuilder DefaultMatch => new MatchDtoBuilder()
            .WithId(TestConstants.DefaultMatchId)
            .WithKickOff(DateTime.Now.AddMinutes(-34))
            .WithHomeTeam(new TeamDetailsDtoBuilder()
                .WithUserId(TestConstants.DefaultUserId)
                .WithSquad(new SquadDtoBuilder()
                    .WithId(TestConstants.DefaultHomeSquadId)
                    .WithName("Default FC")
                    .WithCardsInLineup("ST", new[]
                    {
                        new CardDtoBuilder()
                            .WithId(TestConstants.DefaultHomeActiveCardId)
                            .WithName("Jimmy Johnson")
                    })
                    .WithSubs(new[]
                    {
                        new CardDtoBuilder()
                            .WithId(TestConstants.DefaultHomeSubCardId)
                    })))
            .WithAwayTeam(new TeamDetailsDtoBuilder()
                .WithSquad(new SquadDtoBuilder()
                    .WithName("Evil FC")
                    .WithId(TestConstants.DefaultAwaySquadId)
                    .WithCardsInLineup("ST", new[]
                    {
                        new CardDtoBuilder()
                            .WithId(TestConstants.DefaultAwayActiveCardId)
                    })
                    .WithSubs(new[]
                    {
                        new CardDtoBuilder()
                            .WithId(TestConstants.DefaultAwaySubCardId)
                    })))
            .WithMinute(0, new MatchMinuteDtoBuilder().WithMinuteStats(new MinuteStatsDtoBuilder().WithSquadInPossession(TestConstants.DefaultHomeSquadId))
                .WithEvent(new Event(EventType.ShotOnTarget, TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId)))
            .WithMinute(20, new MatchMinuteDtoBuilder().WithMinuteStats(new MinuteStatsDtoBuilder().WithSquadInPossession(TestConstants.DefaultHomeSquadId))
                .WithEvent(new Event(EventType.Goal, TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId)));
    }
}