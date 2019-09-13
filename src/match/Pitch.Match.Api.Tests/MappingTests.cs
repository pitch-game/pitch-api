using AutoMapper;
using Pitch.Match.Api.Supporting;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class MappingTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            Mapper.Initialize(m => m.AddProfile<AutomapperProfile>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}