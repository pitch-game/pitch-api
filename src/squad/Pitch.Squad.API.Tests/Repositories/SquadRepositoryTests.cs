using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pitch.Squad.API.Infrastructure;
using Pitch.Squad.API.Infrastructure.Repositories;
using Xunit;

namespace Pitch.Squad.API.Tests.Repositories
{
    public class SquadRepositoryTests
    {
        private readonly Mock<IDataContext<Models.Squad>> _mockDataContext;

        public SquadRepositoryTests()
        {
            _mockDataContext = new Mock<IDataContext<Models.Squad>>();
        }

        [Fact]
        public async Task GetAsync_Returns_Squad_By_UserId()
        {
            var userId = Guid.NewGuid().ToString();
            var pack = new Models.Squad
            {
                UserId = userId
            };

            _mockDataContext.Setup(x => x.FindOneAsync(x => x.UserId == userId)).ReturnsAsync(pack);

            var repository = new SquadRepository(_mockDataContext.Object);
            var result = await repository.GetAsync(userId);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            _mockDataContext.Verify(x => x.FindOneAsync(x => x.UserId == userId), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Sets_Id_And_Calls_CreateAsyncOnce_With_Squad()
        {
            var userId = Guid.NewGuid().ToString();

            Models.Squad squad = null;
            _mockDataContext.Setup(x => x.CreateAsync(It.IsAny<Models.Squad>())).Callback<Models.Squad>(s => squad = s);

            var repository = new SquadRepository(_mockDataContext.Object);
            await repository.CreateAsync(userId);

            _mockDataContext.Verify(x => x.CreateAsync(It.Is<Models.Squad>(x => x.UserId == userId)), Times.Once);
            squad.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_Calls_UpdateAsync_With_Squad()
        {
            var userId = Guid.NewGuid().ToString();
            var squad = new Models.Squad
            {
                UserId = userId
            };

            var repository = new SquadRepository(_mockDataContext.Object);
            await repository.UpdateAsync(squad);

            _mockDataContext.Verify(x => x.UpdateAsync(squad), Times.Once);
        }
    }
}
