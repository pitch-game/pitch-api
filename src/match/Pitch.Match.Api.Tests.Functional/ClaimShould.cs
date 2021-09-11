using System;
using System.Threading.Tasks;
using FluentAssertions;
using Pitch.Match.Api.Tests.Functional.Framework;
using Xunit;

namespace Pitch.Match.Api.Tests.Functional
{
    [Collection("Functional")]
    public class ClaimShould
    {
        private readonly TestWebApplicationFactory _testWebApplicationFactory;

        public ClaimShould()
        {
            _testWebApplicationFactory = new TestWebApplicationFactory();
        }

        [Fact]
        public async Task Return_200()
        {
            var client = _testWebApplicationFactory.CreateClient();
            var result = await client.PostAsync("/claim", null);

            result.IsSuccessStatusCode.Should().BeTrue();
            //TODO Verify update?
        }
    }
}
