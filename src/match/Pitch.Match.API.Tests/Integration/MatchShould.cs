using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Pitch.Match.API.Tests.Integration
{
    public class MatchShould : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture _fixture;

        public MatchShould(IntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Return_BadRequest_When_Id_Isnt_Guid()
        {
            var client = _fixture.CreateClient();

            var result = await client.GetAsync("/1");
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Return_Match()
        {
            var client = _fixture.CreateClient();

            var result = await client.GetAsync("/db694e59-ae5c-4e32-989c-e17b0ead81a2"); //TODO DefaultMatchId
            var response = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
        }
    }
}
