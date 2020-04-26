using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Moq;
using Pitch.Store.API.Application.Requests;
using Pitch.Store.API.Application.Responses;
using Pitch.Store.API.Infrastructure.Repositories;
using Pitch.Store.API.Infrastructure.Services;
using Pitch.Store.API.Models;
using Xunit;

namespace Pitch.Store.API.Tests.Services
{
    public class PackServiceTests
    {
        [Fact]
        public async Task GetAllAsyncByUserId_CallsGetAllOnRepository()
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
                UserId = userId,
                Position = "N/A"
            }));

            var mockBus = new Mock<IBus>();
            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            // Act
            var result = await service.Open(packId, Guid.NewGuid().ToString());

            // Assert
            mockPackRepository.Verify(x => x.Delete(packId), Times.Once);
        }

        [Fact]
        public async Task CreateStartingPacks_AddsExactly17Packs()
        {
            var userId = Guid.NewGuid();
            var mockPackRepository = new Mock<IPackRepository>();

            var mockBus = new Mock<IBus>();
            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            // Act
            await service.CreateStartingPacksAsync(userId);

            // Assert
            mockPackRepository.Verify(x => x.AddAsync(It.IsAny<Pack>()), Times.Exactly(17));
        }


        [Fact]
        public async Task Buy_Calls_Add_On_Payment_Success()
        {
            var userId = Guid.NewGuid();
            var amount = 10;

            var mockPackRepository = new Mock<IPackRepository>();

            var takePaymentResponse = new TakePaymentResponse()
            {
                Success = true
            };

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.RequestAsync<TakePaymentRequest, TakePaymentResponse>(It.IsAny<TakePaymentRequest>())).ReturnsAsync(takePaymentResponse);

            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            await service.Buy(userId, amount);

            mockPackRepository.Verify(x => x.AddAsync(It.IsAny<Pack>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Buy_Throws_Exception_On_Payment_Failure()
        {
            var userId = Guid.NewGuid();
            var amount = 10;

            var mockPackRepository = new Mock<IPackRepository>();

            var takePaymentResponse = new TakePaymentResponse()
            {
                Success = false
            };

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.RequestAsync<TakePaymentRequest, TakePaymentResponse>(It.IsAny<TakePaymentRequest>())).ReturnsAsync(takePaymentResponse);

            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            await Assert.ThrowsAsync<Exception>(() => service.Buy(userId, amount));
        }

        [Fact]
        public async Task Buy_Sends_Payment_Request_With_Correct_Params()
        {
            var userId = Guid.NewGuid();
            var amount = 10;

            var mockPackRepository = new Mock<IPackRepository>();

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.RequestAsync<TakePaymentRequest, TakePaymentResponse>(It.IsAny<TakePaymentRequest>())).ReturnsAsync(new TakePaymentResponse()
            {
                Success = true
            });

            var service = new PackService(mockPackRepository.Object, mockBus.Object);

            await service.Buy(userId, amount);

            mockBus.Verify(x => x.RequestAsync<TakePaymentRequest, TakePaymentResponse>(It.Is<TakePaymentRequest>(x => x.UserId == userId && x.Amount == amount)));
        }

    }
}
