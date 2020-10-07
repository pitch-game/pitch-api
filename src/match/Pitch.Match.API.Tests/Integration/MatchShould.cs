using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

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

            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Return_Lineup()
        {
            var result = await _client.GetAsync($"/{TestConstants.DefaultMatchId}/lineup");
            var response = await result.Content.ReadAsStringAsync();

            result.EnsureSuccessStatusCode();
        }
    }
}
