using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pitch.Store.API.Infrastructure;
using Pitch.Store.API.Infrastructure.Repositories;
using Pitch.Store.API.Models;
using Xunit;

namespace Pitch.Store.API.Tests.Repositories
{
    public class PackRepositoryTests
    {
        private readonly Mock<IDataContext<Pack>> _mockDataContext;

        public PackRepositoryTests()
        {
            _mockDataContext = new Mock<IDataContext<Pack>>();
        }

        [Fact]
        public async Task GetAsync_ReturnsExpectedPack()
        {
            var packId = Guid.NewGuid();
            var pack = new Pack
            {
                Id = packId
            };

            _mockDataContext.Setup(x => x.FindOneAsync(x => x.Id == packId)).ReturnsAsync(pack);

            var repository = new PackRepository(_mockDataContext.Object);
            var result = await repository.GetAsync(packId);

            result.Should().NotBeNull();
            result.Id.Should().Be(packId);
            _mockDataContext.Verify(x => x.FindOneAsync(x => x.Id == packId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsExpectedPacks()
        {
            var userId = Guid.NewGuid().ToString();
            var pack = new Pack
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };
            var packs = new[] {pack, pack};

            _mockDataContext.Setup(x => x.FindAsync(x => x.UserId == userId)).ReturnsAsync(packs);

            var repository = new PackRepository(_mockDataContext.Object);
            var results = await repository.GetAllAsync(userId);

            results.Should().BeEquivalentTo(packs);
            _mockDataContext.Verify(x => x.FindAsync(x => x.UserId == userId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_CallsCreateAsyncOnce()
        {
            var packId = Guid.NewGuid();
            var pack = new Pack
            {
                Id = packId
            };

            var repository = new PackRepository(_mockDataContext.Object);
            await repository.AddAsync(pack);

            _mockDataContext.Verify(x => x.CreateAsync(pack), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsDeleteAsyncOnce()
        {
            var packId = Guid.NewGuid();

            var repository = new PackRepository(_mockDataContext.Object);
            await repository.DeleteAsync(packId);

            _mockDataContext.Verify(x => x.DeleteAsync(x => x.Id == packId), Times.Once);
        }
    }
}
