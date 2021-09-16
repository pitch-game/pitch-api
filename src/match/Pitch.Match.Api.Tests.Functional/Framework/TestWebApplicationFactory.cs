using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;
using Pitch.Match.Api.Tests.Functional.Fixtures;

namespace Pitch.Match.Api.Tests.Functional.Framework
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly Mock<IDataContext<Infrastructure.Models.Match>> _mockDataContext;

        private Infrastructure.Models.Match _stubMatch;

        public TestWebApplicationFactory()
        {
            _mockDataContext = new Mock<IDataContext<Infrastructure.Models.Match>>();

            var matchFixtures = new MatchFixtures();
            _stubMatch = matchFixtures.DefaultMatch.Build();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            SetupDefaults();

            builder.ConfigureTestServices(services =>
            {
                ConfigureAuthentication(services);
                ConfigureServices(services);

                var busMock = new Mock<IBus>();
                var pubSubMock = new Mock<IPubSub>();
                busMock.SetupGet(x => x.PubSub).Returns(pubSubMock.Object);
                services.AddSingleton<IBus>(busMock.Object);
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

        private void SetupDefaults()
        {
            _mockDataContext.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Infrastructure.Models.Match, bool>>>()))
                .ReturnsAsync(_stubMatch);

            _mockDataContext
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Infrastructure.Models.Match, bool>>>()))
                .ReturnsAsync(new List<Infrastructure.Models.Match>
                {
                    _stubMatch
                });
        }

        public TestWebApplicationFactory WithMatch(Infrastructure.Models.Match match)
        {
            _stubMatch = match;
            return this;
        }
    }
}
