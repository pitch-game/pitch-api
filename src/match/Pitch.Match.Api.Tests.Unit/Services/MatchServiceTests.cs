using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using FluentAssertions;
using Moq;
using Pitch.Match.Api.ApplicationCore.Models.Matchmaking;
using Pitch.Match.Api.ApplicationCore.Services;
using Pitch.Match.Api.Infrastructure.Mapping;
using Pitch.Match.Api.Infrastructure.MessageBus.Events;
using Pitch.Match.Api.Infrastructure.MessageBus.Requests;
using Pitch.Match.Api.Infrastructure.MessageBus.Responses;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Tests.Shared.Builders;
using Pitch.Match.Engine;
using Pitch.Match.Engine.Services;
using Xunit;
using Event = Pitch.Match.Api.Infrastructure.Models.Event;
using EventType = Pitch.Match.Api.Infrastructure.Models.EventType;

namespace Pitch.Match.Api.Tests.Unit.Services
{
    public class MatchServiceTests
    {
        private MatchService _matchService;

        private readonly Mock<IMatchmakingService> _matchMatchingServiceMock = new();
        private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
        private readonly Mock<IMatchMapper> _matchMapperMock = new();
        private readonly Mock<IMatchDtoMapper> _matchDtoMapperMock = new();
        
        [Fact]
        public async Task ClaimingMatchReward_AsAwayTeam_ShouldMarkMatchAsClaimedByAwayTeam()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var homeSquadId = Guid.NewGuid();
            var cardId = Guid.NewGuid();

            var unclaimedMatchDto = new MatchDtoBuilder()
                .WithId(matchId)
                .WithAwayTeam(new TeamDetailsDtoBuilder()
                    .WithSquad(new SquadDtoBuilder()
                        .WithId(homeSquadId))
                    .WithUserId(userId)
                    .WithHasClaimedRewards(false))
                .WithMinute(30, new MatchMinuteDtoBuilder().WithMinuteStats(new MinuteStatsDtoBuilder().WithSquadInPossession(homeSquadId))
                    .WithEvent(new Event(EventType.Goal,cardId, homeSquadId)))
                .Build();

            var unclaimedMatch = new MatchBuilder()
                .WithId(matchId)
                .WithAwayTeam(new TeamDetailsBuilder()
                    .WithSquad(new SquadBuilder()
                        .WithId(homeSquadId))
                    .WithUserId(userId)
                    .WithHasClaimedRewards(false))
                .WithMinute(30, new MatchMinuteBuilder().WithMinuteStats(new MinuteStatsBuilder().WithSquadInPossession(homeSquadId))
                    .WithEvent(new Engine.Models.Event(Engine.Models.EventType.Goal, cardId, homeSquadId)))
                .Build();

            Infrastructure.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            _matchRepositoryMock.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<Infrastructure.Models.Match>)new List<Infrastructure.Models.Match>
                    {unclaimedMatchDto}));
            _matchRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Infrastructure.Models.Match>()))
                .Callback<Infrastructure.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(unclaimedMatchDto)).Returns(unclaimedMatch);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(unclaimedMatchDto);

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PubSub.PublishAsync(It.IsAny<MatchCompletedEvent>(), It.IsAny<Action<IPublishConfiguration>>(), It.IsAny<CancellationToken>()))
                .Callback<MatchCompletedEvent, Action<IPublishConfiguration>, CancellationToken>((mce, config, ct) => publishedEvent = mce);

            _matchService = new MatchService(_matchMatchingServiceMock.Object, stubMatchEngine.Object,
                _matchRepositoryMock.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            await _matchService.ClaimAsync(userId);

            _matchMapperMock.Verify(x => x.Map(It.Is<Engine.Models.Match>(match => match.AwayTeam.HasClaimedRewards)), Times.Once);
            _matchRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Infrastructure.Models.Match>()), Times.Once);

            publishedEvent.UserId.Should().Be(userId);
            publishedEvent.MatchId.Should().Be(matchId);
            publishedEvent.Victorious.Should().BeTrue();
            publishedEvent.Scorers.Count.Should().Be(1);
        }

        [Fact]
        public async Task ClaimingMatchReward_AsHomeTeam_ShouldMarkMatchAsClaimedByHomeTeam()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var homeSquadId = Guid.NewGuid();
            var cardId = Guid.NewGuid();

            var unclaimedMatchDto = new MatchDtoBuilder()
                .WithId(matchId)
                .WithHomeTeam(new TeamDetailsDtoBuilder()
                    .WithSquad(new SquadDtoBuilder()
                        .WithId(homeSquadId))
                    .WithUserId(userId)
                    .WithHasClaimedRewards(false))
                .WithMinute(30, new MatchMinuteDtoBuilder().WithMinuteStats(new MinuteStatsDtoBuilder().WithSquadInPossession(homeSquadId))
                    .WithEvent(new Event(EventType.Goal, cardId, homeSquadId)))
                .Build();

            var unclaimedMatch = new MatchBuilder()
                .WithId(matchId)
                .WithHomeTeam(new TeamDetailsBuilder()
                    .WithSquad(new SquadBuilder()
                        .WithId(homeSquadId))
                    .WithUserId(userId)
                    .WithHasClaimedRewards(false))
                .WithMinute(30, new MatchMinuteBuilder().WithMinuteStats(new MinuteStatsBuilder().WithSquadInPossession(homeSquadId))
                    .WithEvent(new Engine.Models.Event(Engine.Models.EventType.Goal, cardId, homeSquadId)))
                .Build();

            Infrastructure.Models.Match updatedMatch = null;
            MatchCompletedEvent publishedEvent = null;

            _matchRepositoryMock.Setup(x => x.GetUnclaimedAsync(userId)).Returns(
                Task.FromResult((IEnumerable<Infrastructure.Models.Match>)new List<Infrastructure.Models.Match>
                    {unclaimedMatchDto}));
            _matchRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Infrastructure.Models.Match>()))
                .Callback<Infrastructure.Models.Match>(r => updatedMatch = r);

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(unclaimedMatchDto)).Returns(unclaimedMatch);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(unclaimedMatchDto);

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PubSub.PublishAsync(It.IsAny<MatchCompletedEvent>(), It.IsAny<Action<IPublishConfiguration>>(), It.IsAny<CancellationToken>()))
                .Callback<MatchCompletedEvent, Action<IPublishConfiguration>, CancellationToken>((mce, config, ct) => publishedEvent = mce);

            _matchService = new MatchService(_matchMatchingServiceMock.Object, stubMatchEngine.Object,
                _matchRepositoryMock.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            await _matchService.ClaimAsync(userId);

            _matchMapperMock.Verify(x => x.Map(It.Is<Engine.Models.Match>(match => match.HomeTeam.HasClaimedRewards)), Times.Once);
            _matchRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Infrastructure.Models.Match>()), Times.Once);

            publishedEvent.UserId.Should().Be(userId);
            publishedEvent.MatchId.Should().Be(matchId);
            publishedEvent.Victorious.Should().BeTrue();
            publishedEvent.Scorers.Count.Should().Be(1);
        }

        [Fact]
        public async Task Get_ReturnsMatch()
        {
            var matchId = Guid.NewGuid();
            var matchDto = new Infrastructure.Models.Match { Id = matchId };
            var match = new Engine.Models.Match { Id = matchId };

            _matchDtoMapperMock.Setup(x => x.Map(matchDto)).Returns(match);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(matchDto);

            _matchRepositoryMock.Setup(x => x.GetAsync(matchId)).Returns(Task.FromResult(matchDto));

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(_matchMatchingServiceMock.Object, stubMatchEngine.Object,
                _matchRepositoryMock.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            var returnedMatch = await _matchService.GetAsAtElapsedAsync(matchId);

            returnedMatch.Should().BeEquivalentTo(match);
            mockCalculatedStatService.Verify(x => x.Set(It.IsAny<Engine.Models.Match>()), Times.Once);
        }

        [Fact]
        public async Task KickOff_ReturnsMatch()
        {
            var sessionId = Guid.NewGuid();
            var hostPlayerId = Guid.NewGuid();
            var joinedPlayerId = Guid.NewGuid();

            var session = new MatchmakingSession
            {
                Id = sessionId,
                HostPlayerId = hostPlayerId,
                JoinedPlayerId = joinedPlayerId
            };
            _matchMatchingServiceMock.Setup(x => x.GetSession(sessionId)).Returns(session);

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.Simulate(It.IsAny<Match.Engine.Models.Match>()))
                .Returns<Match.Engine.Models.Match>(x => x);

            Infrastructure.Models.Match simulatedMatch = null;

            var matchDto = new MatchDtoBuilder().Build();

            var sub = new CardBuilder().Build();
            var lst = new CardBuilder().Build();
            var gk = new CardBuilder().Build();
            var lb = new CardBuilder().Build();
            var lm = new CardBuilder().Build();

            var mockBus = new Mock<IBus>();
            var squadResponse = new GetSquadResponse
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Subs = new [] { sub },
                Lineup = new Dictionary<string, Engine.Models.Card>()
                {
                    {"GK", gk},
                    {"LB", lb},
                    {"LM", lm},
                    {"LST", lst}
                }
            };
            mockBus.Setup(x => x.Rpc.RequestAsync<GetSquadRequest, GetSquadResponse>(It.IsAny<GetSquadRequest>(), It.IsAny<Action<IRequestConfiguration>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(squadResponse);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.CreateAsync(It.IsAny<Infrastructure.Models.Match>()))
                .Callback<Infrastructure.Models.Match>(r => simulatedMatch = r);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(matchDto);

            _matchService = new MatchService(_matchMatchingServiceMock.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            await _matchService.KickOff(sessionId);

            simulatedMatch.Should().Be(matchDto);
            mockBus.Verify(x => x.Rpc.RequestAsync<GetSquadRequest, GetSquadResponse>(It.Is<GetSquadRequest>(x => x.UserId == hostPlayerId), It.IsAny<Action<IRequestConfiguration>>(), It.IsAny<CancellationToken>()), Times.Once);
            mockBus.Verify(x => x.Rpc.RequestAsync<GetSquadRequest, GetSquadResponse>(It.Is<GetSquadRequest>(x => x.UserId == joinedPlayerId), It.IsAny<Action<IRequestConfiguration>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Substitution_CallsMatchSubstituteMethodOnce_AndIncrementsSubCount()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var playerId = Guid.NewGuid();
            var subId = Guid.NewGuid();

            var match = new MatchBuilder()
                .WithHomeTeam(new TeamDetailsBuilder()
                    .WithUserId(userId)
                    .WithUsedSubs(0)
                    .WithSquad(new SquadBuilder()
                        .WithCardsInLineup("ST", new[] { new CardBuilder().WithId(playerId) })
                        .WithSubs(new[] { new CardBuilder().WithId(subId) })
                    )
                )
                .Build();

            var matchDto = new MatchDtoBuilder()
                .WithHomeTeam(new TeamDetailsDtoBuilder()
                    .WithUserId(userId)
                    .WithUsedSubs(0)
                    .WithSquad(new SquadDtoBuilder()
                        .WithCardsInLineup("ST", new []{new CardDtoBuilder().WithId(playerId)})
                        .WithSubs(new []{new CardDtoBuilder().WithId(subId)})
                    )
                )
                .Build();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            stubMatchEngine.Setup(x => x.Simulate(It.IsAny<Engine.Models.Match>())).Returns(match);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(matchDto);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(matchDto)).Returns(match);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(matchDto);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            await _matchService.Substitution(playerId, subId, matchId, userId);

            stubMatchEngine.Verify(x => x.Simulate(It.Is<Engine.Models.Match>(x => x.HomeTeam.UsedSubs == 1)));
        }

        [Fact]
        public async Task Substitution_WhenAllSubsUsed_ThrowsException()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var match = new MatchBuilder()
                .WithHomeTeam(new TeamDetailsBuilder().WithUserId(userId).WithUsedSubs(MatchService.SubCount)).Build();

            var matchDto = new MatchDtoBuilder()
                .WithHomeTeam(new TeamDetailsDtoBuilder().WithUserId(userId).WithUsedSubs(MatchService.SubCount)).Build();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();
            //stubMatchEngine.Setup(x => x.Simulate(It.IsAny<Engine.Models.Match>()))
            //    .Returns(match);

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(matchId)).ReturnsAsync(matchDto);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(matchDto)).Returns(match);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            await Assert.ThrowsAsync<Exception>(() => _matchService.Substitution(Guid.NewGuid(), Guid.NewGuid(), matchId, userId));
        }

        [Fact]
        public async Task GetMatchStatus_ReturnsCorrectModel()
        {
            var userId = Guid.NewGuid();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.HasUnclaimedAsync(userId)).ReturnsAsync(true);
            mockMatchRepository.Setup(x => x.GetInProgressAsync(userId)).ReturnsAsync((Guid?)null);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            var result = await _matchService.GetMatchStatus(userId);

            result.HasUnclaimedRewards.Should().BeTrue();
            result.InProgressMatchId.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_CallsGetAllAsyncOnce()
        {
            var userId = Guid.NewGuid();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), userId)).ReturnsAsync(new List<Infrastructure.Models.Match>());

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            var result = await _matchService.GetAllAsync(0, 1, userId);

            mockMatchRepository.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), userId), Times.Once);
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetLineupAsync_Returns_Correct_Lineup()
        {
            var userId = Guid.NewGuid();
            var matchId = Guid.NewGuid();

            var matchDto = new MatchDtoBuilder()
                .WithHomeTeam(new TeamDetailsDtoBuilder()
                    .WithUserId(userId)
                    .WithSquad(new SquadDtoBuilder().WithSubs(new[]
                    {
                        new CardDtoBuilder()
                    }))
                    )
                .Build();

            var match = new MatchBuilder()
                .WithHomeTeam(new TeamDetailsBuilder()
                    .WithUserId(userId)
                    .WithSquad(new SquadBuilder().WithSubs(new[]
                    {
                        new CardBuilder()
                    }))
                )
                .Build();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(matchDto);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(matchDto)).Returns(match);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(matchDto);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            var lineup = await _matchService.GetLineupAsync(matchId, userId);

            mockMatchRepository.Verify(x => x.GetAsync(matchId), Times.Once);
            lineup.Active.Should().BeEquivalentTo(match.HomeTeam.Squad.Lineup.SelectMany(x => x.Value));
            lineup.Subs.Should().BeEquivalentTo(match.HomeTeam.Squad.Subs);
        }

        [Fact]
        public async Task GetLineupAsync_Excludes_SentOffPlayers()
        {
            var userId = Guid.NewGuid();
            var squadId = Guid.NewGuid();
            var matchId = Guid.NewGuid();
            var cardId = Guid.NewGuid();

            var eventMinute = 20;

            var matchDto = new MatchDtoBuilder()
                .WithKickOff(DateTime.Now.AddMinutes(-eventMinute - 1))
                .WithHomeTeam(new TeamDetailsDtoBuilder()
                    .WithUserId(userId)
                    .WithSquad(new SquadDtoBuilder()
                        .WithId(squadId)
                        .WithCardsInLineup("ST", new[]
                            {
                                new CardDtoBuilder()
                                    .WithId(cardId)
                            }
                        )))
                .WithMinute(eventMinute, new MatchMinuteDtoBuilder().WithMinuteStats(new MinuteStatsDtoBuilder().WithSquadInPossession(squadId))
                    .WithEvent(new Event(EventType.RedCard, cardId, squadId)))
                .Build();

            var match = new MatchBuilder()
                .WithKickOff(DateTime.Now.AddMinutes(-eventMinute - 1))
                .WithHomeTeam(new TeamDetailsBuilder()
                    .WithUserId(userId)
                    .WithSquad(new SquadBuilder()
                        .WithId(squadId)
                        .WithCardsInLineup("ST", new[]
                            {
                                new CardBuilder()
                                    .WithId(cardId)
                            }
                        )))
                .WithMinute(eventMinute, new MatchMinuteBuilder().WithMinuteStats(new MinuteStatsBuilder().WithSquadInPossession(squadId))
                    .WithEvent(new Engine.Models.Event(Engine.Models.EventType.RedCard, cardId, squadId)))
                .Build();

            var mockMatchmakingService = new Mock<IMatchmakingService>();

            var stubMatchEngine = new Mock<IMatchEngine>();

            var mockBus = new Mock<IBus>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(matchDto);

            var mockCalculatedStatService = new Mock<ICalculatedCardStatService>();

            _matchDtoMapperMock.Setup(x => x.Map(matchDto)).Returns(match);
            _matchMapperMock.Setup(x => x.Map(It.IsAny<Engine.Models.Match>())).Returns(matchDto);

            _matchService = new MatchService(mockMatchmakingService.Object, stubMatchEngine.Object,
                mockMatchRepository.Object, mockBus.Object, mockCalculatedStatService.Object, _matchDtoMapperMock.Object, _matchMapperMock.Object);

            var lineup = await _matchService.GetLineupAsync(matchId, userId);

            mockMatchRepository.Verify(x => x.GetAsync(matchId), Times.Once);
            lineup.Active.Should().BeEmpty();
        }
    }
}