using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
using Pitch.Match.API.Tests.Builders;
using Pitch.Match.API.Tests.Integration;

namespace Pitch.Match.API.Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly Mock<IDataContext<ApplicationCore.Models.Match.Match>> _mockDataContext;

        private ApplicationCore.Models.Match.Match _stubMatch;

        public TestWebApplicationFactory()
        {
            _mockDataContext = new Mock<IDataContext<ApplicationCore.Models.Match.Match>>();
            SetDefaults();
        }

        private void SetDefaults()
        {
            _stubMatch = new MatchBuilder()
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
                                .Build()
                        })
                        .WithSubs(new[]
                        {
                            new CardBuilder()
                                .WithId(TestConstants.DefaultHomeSubCardId)
                                .Build()
                        })
                        .Build())
                    .Build())
                .WithAwayTeam(new TeamDetailsBuilder()
                    .WithSquad(new SquadBuilder()
                        .WithName("Evil FC")
                        .WithId(TestConstants.DefaultAwaySquadId)
                        .WithCardsInLineup("ST", new[]
                        {
                            new CardBuilder()
                                .WithId(TestConstants.DefaultAwayActiveCardId)
                                .Build()
                        })
                        .WithSubs(new []
                        {
                            new CardBuilder()
                                .WithId(TestConstants.DefaultAwaySubCardId)
                                .Build()
                        })
                        .Build())
                    .Build())
                .WithMinute(0, new MatchMinuteBuilder(TestConstants.DefaultHomeSquadId)
                    .WithEvent(new ShotOnTarget(TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId))
                    .Build())
                .WithMinute(20, new MatchMinuteBuilder(TestConstants.DefaultHomeSquadId)
                    .WithEvent(new Goal(TestConstants.DefaultHomeActiveCardId, TestConstants.DefaultHomeSquadId))
                    .Build()
                )
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            SetupMocks();

            builder.ConfigureTestServices(services =>
            {
                ConfigureAuthentication(services);
                ConfigureServices(services);
            });
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped((sp) => _mockDataContext.Object);
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", options => { });
        }

        private void SetupMocks()
        {
            _mockDataContext.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match.Match, bool>>>()))
                .ReturnsAsync(_stubMatch);

            _mockDataContext
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match.Match, bool>>>()))
                .ReturnsAsync(new List<ApplicationCore.Models.Match.Match>
                {
                    _stubMatch
                });
        }

        public TestWebApplicationFactory SetMatch(ApplicationCore.Models.Match.Match match)
        {
            _stubMatch = match;
            return this;
        }
    }
}
