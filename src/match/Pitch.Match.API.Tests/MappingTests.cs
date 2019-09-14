using AutoMapper;
using Pitch.Match.API.Supporting;
using Xunit;

namespace Pitch.Match.API.Tests
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