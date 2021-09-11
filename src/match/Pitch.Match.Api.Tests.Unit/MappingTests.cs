using AutoMapper;
using Pitch.Match.API.Supporting;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit
{
    public class MappingTests
    {
        [Fact]
        [System.Obsolete]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}