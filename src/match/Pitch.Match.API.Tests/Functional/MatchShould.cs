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
using Pitch.Match.API.Tests.Builders;
using Pitch.Match.API.Tests.Functional.Fixtures;
using Pitch.Match.API.Tests.Functional.Framework;
using Xunit;

namespace Pitch.Match.API.Tests.Functional
{
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
            //responseModel.Match.Should().BeEquivalentTo(_matchFixtures.DefaultMatchResultModel);
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
            //responseModel.Should().BeEquivalentTo(_matchFixtures.DefaultLineupModel);
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
            _testWebApplicationFactory?.Dispose();
        }
    }
}
