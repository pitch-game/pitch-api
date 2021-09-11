using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.Api.Tests.Functional.Framework;
using Pitch.Match.Api.Tests.Shared;
using Xunit;

namespace Pitch.Match.Api.Tests.Functional
{
    [Collection("Functional")]
    public class StatusShould : IDisposable
    {
        private readonly TestWebApplicationFactory _testWebApplicationFactory;
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public StatusShould()
        { 
            _testWebApplicationFactory = new TestWebApplicationFactory();
        }

        [Fact]
        public async Task Return_Status()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.GetAsync($"/status");
            var response = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<MatchStatusResult>(response, JsonSerializerOptions);

            responseModel.Should().NotBeNull();
            responseModel.HasUnclaimedRewards.Should().BeTrue();
            responseModel.InProgressMatchId.Should().Be(TestConstants.DefaultMatchId);
        }

        public void Dispose()
        {
            _testWebApplicationFactory?.Dispose();
        }
    }
}
