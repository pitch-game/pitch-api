﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
using Pitch.Match.API.Tests.Functional.Fixtures;

namespace Pitch.Match.API.Tests.Functional.Framework
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly Mock<IDataContext<ApplicationCore.Models.Match.Match>> _mockDataContext;

        private ApplicationCore.Models.Match.Match _stubMatch;

        public TestWebApplicationFactory()
        {
            _mockDataContext = new Mock<IDataContext<ApplicationCore.Models.Match.Match>>();

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
            _mockDataContext.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match.Match, bool>>>()))
                .ReturnsAsync(_stubMatch);

            _mockDataContext
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match.Match, bool>>>()))
                .ReturnsAsync(new List<ApplicationCore.Models.Match.Match>
                {
                    _stubMatch
                });
        }

        public TestWebApplicationFactory WithMatch(ApplicationCore.Models.Match.Match match)
        {
            _stubMatch = match;
            return this;
        }
    }
}
