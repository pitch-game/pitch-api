using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.Match;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
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

            //TODO default match builder
            var matchId = new Guid("db694e59-ae5c-4e32-989c-e17b0ead81a2");
            var match = new ApplicationCore.Models.Match.Match()
            {
                Id = matchId,
                KickOff = DateTime.Now.AddMinutes(-5),
                HomeTeam = new TeamDetails()
                {
                    Squad = new Squad()
                },
                AwayTeam = new TeamDetails()
                {
                    Squad = new Squad()
                }
            };

            _mockDataContext.Setup(x => x.FindOneAsync(x => x.Id == matchId))
                .ReturnsAsync(match);

            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                    { 
                        options.DefaultAuthenticateScheme = "Test"; 
                        options.DefaultChallengeScheme = "Test";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });

                services.AddScoped((sp) => _mockDataContext.Object);
            });
        }
    }
}
