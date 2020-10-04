﻿using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
using Pitch.Match.API.Tests.Builders;
using Pitch.Match.API.Tests.Integration;

namespace Pitch.Match.API.Tests
{
    public class IntegrationTestsFixture : WebApplicationFactory<Startup>
    {
        private readonly Mock<IDataContext<ApplicationCore.Models.Match.Match>> _mockDataContext;

        public IntegrationTestsFixture()
        {
            _mockDataContext = new Mock<IDataContext<ApplicationCore.Models.Match.Match>>();
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
            var match = new MatchBuilder()
                .WithId(new Guid("db694e59-ae5c-4e32-989c-e17b0ead81a2"))
                .WithHomeTeam(new TeamDetailsBuilder().WithUserId(new Guid("6338a6e5-eaaa-4299-9428-763d85363c3e")).Build())
                .Build(); //TODO DEFAULT MATCH ID

            _mockDataContext.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match.Match, bool>>>()))
                .ReturnsAsync(match);
        }
    }
}
