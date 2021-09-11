using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.Models;
using Pitch.Match.Api.Tests.Functional.Fixtures;
using Pitch.Match.Api.Tests.Functional.Framework;
using Pitch.Match.Api.Tests.Shared;
using Xunit;

namespace Pitch.Match.Api.Tests.Functional
{
    [Collection("Functional")]
    public class MatchShould : IClassFixture<MatchFixtures>, IDisposable
    {
        private readonly TestWebApplicationFactory _testWebApplicationFactory;
        private readonly MatchFixtures _matchFixtures;
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public MatchShould(MatchFixtures matchFixtures)
        {
            _matchFixtures = matchFixtures;
            _testWebApplicationFactory = new TestWebApplicationFactory();
        }

        [Fact]
        public async Task Return_MatchListResult()
        {
            var kickOff = DateTime.Now.AddDays(-1);
            var finishedMatch = _matchFixtures.DefaultMatch.WithKickOff(kickOff).Build();

            var client = _testWebApplicationFactory.WithMatch(finishedMatch).CreateClient();
            var result = await client.GetAsync($"/?skip=0&take=10");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<IEnumerable<MatchListResultModel>>(response, _jsonSerializerOptions);

            result.EnsureSuccessStatusCode();
            responseModel.Should().HaveCount(1);
            responseModel.First().AwayScore.Should().Be(0);
            responseModel.First().HomeScore.Should().Be(1);
            responseModel.First().Id.Should().Be(TestConstants.DefaultMatchId);
            responseModel.First().Result.Should().Be("W");
            responseModel.First().Claimed.Should().BeFalse();
            responseModel.First().KickOff.Should().Be(kickOff);
            responseModel.First().HomeTeam.Should().Be("Default FC");
            responseModel.First().AwayTeam.Should().Be("Evil FC");
        }

        [Fact]
        public async Task Return_BadRequest_When_Id_Isnt_Guid()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.GetAsync("/1");
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Return_Match()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.GetAsync($"/{TestConstants.DefaultMatchId}");
            var response = await result.Content.ReadAsStringAsync();
            _jsonSerializerOptions = new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true};
            var responseModel = JsonSerializer.Deserialize<MatchModel>(response, _jsonSerializerOptions);

            result.EnsureSuccessStatusCode();

            responseModel.Match.Should().NotBeNull();

            responseModel.Match.AwayResult.Should().NotBeNull();
            responseModel.Match.AwayResult.Name.Should().Be("Evil FC");
            responseModel.Match.AwayResult.Score.Should().Be(0);
            responseModel.Match.AwayResult.Scorers.Count().Should().Be(0);

            responseModel.Match.HomeResult.Should().NotBeNull();
            responseModel.Match.HomeResult.Name.Should().Be("Default FC");
            responseModel.Match.HomeResult.Score.Should().Be(1);
            responseModel.Match.HomeResult.Scorers.Count().Should().Be(1);
            responseModel.Match.HomeResult.Scorers.First().Should().Be("Jimmy Johnson 20'");

            responseModel.Match.AwayStats.Should().NotBeNull();
            responseModel.Match.AwayStats.Fouls.Should().Be(0);
            responseModel.Match.AwayStats.Possession.Should().Be(0);
            responseModel.Match.AwayStats.RedCards.Should().Be(0);
            responseModel.Match.AwayStats.Shots.Should().Be(0);
            responseModel.Match.AwayStats.ShotsOnTarget.Should().Be(0);
            responseModel.Match.AwayStats.YellowCards.Should().Be(0);

            responseModel.Match.HomeStats.Should().NotBeNull();
            responseModel.Match.HomeStats.Fouls.Should().Be(0);
            responseModel.Match.HomeStats.Possession.Should().Be(6);
            responseModel.Match.HomeStats.RedCards.Should().Be(0);
            responseModel.Match.HomeStats.Shots.Should().Be(2);
            responseModel.Match.HomeStats.ShotsOnTarget.Should().Be(2);
            responseModel.Match.HomeStats.YellowCards.Should().Be(0);

            responseModel.Match.Expired.Should().BeFalse();
            responseModel.Match.ExpiredOn.Should().BeNull();
            responseModel.Match.Minute.Should().Be(34);
            
            //lineup

            //timeline

            responseModel.SubsRemaining.Should().Be(3);
        }

        [Fact]
        public async Task Return_Lineup()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.GetAsync($"/{TestConstants.DefaultMatchId}/lineup");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<LineupModel>(response, _jsonSerializerOptions);

            result.EnsureSuccessStatusCode();

            responseModel.Should().NotBeNull();

            responseModel.Active.Count.Should().Be(1);
            responseModel.Active.First().Id.Should().Be(TestConstants.DefaultHomeActiveCardId);
            responseModel.Active.First().Name.Should().Be("Jimmy Johnson");
            responseModel.Active.First().Rating.Should().Be(50);
            responseModel.Active.First().Fitness.Should().Be(100);

            responseModel.Subs.Length.Should().Be(1);
            responseModel.Subs.First().Id.Should().Be(TestConstants.DefaultHomeSubCardId);
            responseModel.Subs.First().Rating.Should().Be(50);
            responseModel.Subs.First().Fitness.Should().Be(100);
        }

        [Fact]
        public async Task Return_200_Status_Code_On_Substitution()
        {
            var subRequest = new SubRequest()
            {
                Off = TestConstants.DefaultHomeActiveCardId,
                On = TestConstants.DefaultHomeSubCardId
            };
            var httpContent = new StringContent(JsonSerializer.Serialize(subRequest), Encoding.UTF8, "application/json");
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.PostAsync($"/{TestConstants.DefaultMatchId}/substitution", httpContent);

            result.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            //_testWebApplicationFactory?.Dispose();
        }
    }
}
