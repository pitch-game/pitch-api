using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Pitch.Match.API.Models;
using Xunit;
using System.Text.Json;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.Tests.Builders;

namespace Pitch.Match.API.Tests.Integration
{
    public class MatchShould : IClassFixture<MatchResponseFixtures>, IDisposable
    {
        private readonly TestWebApplicationFactory _testWebApplicationFactory;
        private readonly MatchResponseFixtures _matchResponseFixtures;

        public MatchShould(MatchResponseFixtures matchResponseFixtures)
        {
            _matchResponseFixtures = matchResponseFixtures;
            _testWebApplicationFactory = new TestWebApplicationFactory();
        }

        [Fact]
        public async Task Return_MatchListResult()
        {
            var finishedMatch = new MatchBuilder()
                .WithKickOff(DateTime.Now.AddDays(-1))
                .WithId(TestConstants.DefaultMatchId)
                .WithHomeTeam(new TeamDetailsBuilder()
                    .WithUserId(TestConstants.DefaultUserId)
                    .Build())
                .Build();

            var client = _testWebApplicationFactory.SetMatch(finishedMatch).CreateClient();
            var result = await client.GetAsync($"/?skip=0&take=10");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<IEnumerable<MatchListResultModel>>(response);

            result.EnsureSuccessStatusCode();
            responseModel.Should().HaveCount(1);
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
            var responseModel = JsonSerializer.Deserialize<MatchModel>(response, new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true});

            result.EnsureSuccessStatusCode();
            responseModel.Match.Should().BeEquivalentTo(_matchResponseFixtures.DefaultMatchResultModel);
        }

        [Fact]
        public async Task Return_Lineup()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.GetAsync($"/{TestConstants.DefaultMatchId}/lineup");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<LineupModel>(response);

            result.EnsureSuccessStatusCode();
            responseModel.Subs.Should().BeNull();
            responseModel.Active.Should().BeNull();
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
