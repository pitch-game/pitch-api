using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Moq;
using Pitch.Store.API.Infrastructure.Repositories;
using Pitch.Store.API.Infrastructure.Services;
using Pitch.Store.API.Models;
using Xunit;

namespace Pitch.Store.API.Tests.Services
{
    public class PackServiceTests
    {
        [Fact]
        public async Task GetAllAsyncByUserId_ReturnsPacks()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var mockPackRepository = new Mock<IPackRepository>();

            var mockBus = new Mock<IBus>();
            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            // Act
            var result = await service.GetAll(userId);

            // Assert
            mockPackRepository.Verify(x => x.GetAllAsync(userId), Times.Once);
        }

        [Fact]
        public async Task OpenAsync_CallsDeleteOnce()
        {
            // Arrange
            var packId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            var mockPackRepository = new Mock<IPackRepository>();
            mockPackRepository.Setup(x => x.GetAsync(packId)).Returns(Task.FromResult(new Pack()
            {
                Id = packId,
                UserId = userId
            }));

            var mockBus = new Mock<IBus>();
            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            // Act
            var result = await service.Open(packId, Guid.NewGuid().ToString());

            // Assert
            mockPackRepository.Verify(x => x.Delete(packId), Times.Once);
        }
    }
}
