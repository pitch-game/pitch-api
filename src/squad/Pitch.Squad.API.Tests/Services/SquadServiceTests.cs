using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Pitch.Squad.API.Exceptions;
using Pitch.Squad.API.Infrastructure.Repositories;
using Pitch.Squad.API.Services;
using Xunit;

namespace Pitch.Squad.API.Tests.Services
{
    public class SquadServiceTests
    {
        private readonly Mock<ISquadRepository> _mockSquadRepository;
        private readonly Mock<ISquadValidationService> _mockSquadValidationService;

        public SquadServiceTests()
        {
            _mockSquadRepository = new Mock<ISquadRepository>();
            _mockSquadValidationService = new Mock<ISquadValidationService>();
        }

        [Fact]
        public async Task Return_Active_Squad_If_Exists()
        {
            Fixture fixture = new Fixture();
            var squadFixture = fixture.Create<Models.Squad>();

            _mockSquadRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(squadFixture);

            var squadService = new SquadService(_mockSquadRepository.Object, _mockSquadValidationService.Object);

            var squad = await squadService.GetOrCreateAsync(squadFixture.UserId);
            squad.Should().Be(squadFixture);
        }

        [Fact]
        public async Task Return_Created_Squad_If_Doesnt_Exist()
        {
            Fixture fixture = new Fixture();
            var squadFixture = fixture.Create<Models.Squad>();

            _mockSquadRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((Models.Squad)null);
            _mockSquadRepository.Setup(x => x.CreateAsync(It.IsAny<string>())).ReturnsAsync(squadFixture);

            var squadService = new SquadService(_mockSquadRepository.Object, _mockSquadValidationService.Object);

            var squad = await squadService.GetOrCreateAsync(squadFixture.UserId);
            squad.Should().Be(squadFixture);
        }

        [Fact]
        public async Task Update_If_Squad_Is_Valid_On_Update()
        {
            Fixture fixture = new Fixture();
            var existingSquadFixture = fixture.Create<Models.Squad>();
            var userId = fixture.Create<string>();

            var updatedSquadFixture = fixture.Create<Models.Squad>();
            
            _mockSquadRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(existingSquadFixture);
            _mockSquadRepository.Setup(x => x.UpdateAsync(updatedSquadFixture)).ReturnsAsync(updatedSquadFixture);
            _mockSquadValidationService.Setup(x => x.Validate(updatedSquadFixture, existingSquadFixture.Id, userId)).ReturnsAsync(true);

            var squadService = new SquadService(_mockSquadRepository.Object, _mockSquadValidationService.Object);

            var squad = await squadService.UpdateAsync(updatedSquadFixture, userId);
            squad.Should().Be(updatedSquadFixture);

            _mockSquadRepository.Verify(x => x.GetAsync(userId), Times.Once);
            _mockSquadRepository.Verify(x => x.UpdateAsync(updatedSquadFixture), Times.Once);
        }

        [Fact]
        public async Task Throw_Exception_If_Squad_Is_Invalid_On_Update()
        {
            Fixture fixture = new Fixture();
            var existingSquadFixture = fixture.Create<Models.Squad>();
            var userId = fixture.Create<string>();

            var updatedSquadFixture = fixture.Create<Models.Squad>();

            _mockSquadRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(existingSquadFixture);
            _mockSquadValidationService.Setup(x => x.Validate(updatedSquadFixture, existingSquadFixture.Id, userId)).ReturnsAsync(false);

            var squadService = new SquadService(_mockSquadRepository.Object, _mockSquadValidationService.Object);

            Func<Task> act = async () => { await squadService.UpdateAsync(updatedSquadFixture, userId); };
            await act.Should().ThrowAsync<InvalidSquadException>("Squad is not valid.");

            _mockSquadRepository.Verify(x => x.GetAsync(userId), Times.Once);
            _mockSquadRepository.Verify(x => x.UpdateAsync(updatedSquadFixture), Times.Never);
        }
    }
}
