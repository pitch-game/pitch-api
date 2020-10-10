using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Pitch.Match.API.Models;
using Xunit;
using System.Text.Json;

namespace Pitch.Match.API.Tests.Integration
{
    public class MatchShould : IClassFixture<IntegrationTestsFixture>
    {
        private readonly HttpClient _client;

        public MatchShould(IntegrationTestsFixture fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Return_BadRequest_When_Id_Isnt_Guid()
        {
            var result = await _client.GetAsync("/1");
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Return_Match()
        {
            var result = await _client.GetAsync($"/{TestConstants.DefaultMatchId}");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<MatchModel>(response);

            result.EnsureSuccessStatusCode();
            responseModel.SubsRemaining.Should().Be(0);
            responseModel.Match.Should().BeNull();
        }

        [Fact]
        public async Task Return_Lineup()
        {
            var result = await _client.GetAsync($"/{TestConstants.DefaultMatchId}/lineup");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<LineupModel>(response);

            result.EnsureSuccessStatusCode();
            responseModel.Subs.Should().BeNull();
            responseModel.Active.Should().BeNull();
        }
    }
}
