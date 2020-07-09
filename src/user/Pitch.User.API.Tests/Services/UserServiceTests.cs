using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Moq;
using Pitch.User.API.Application.Events;
using Pitch.User.API.Infrastructure.Repositories;
using Pitch.User.API.Services;
using RabbitMQ.Client.Framing.Impl;
using Xunit;

namespace Pitch.User.API.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAsyncById_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Models.User
            {
                Id = userId
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(user);

            var mockBus = new Mock<IBus>();
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            var actualUser = await service.GetAsync(userId);

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public async Task GetAsyncByEmail_ReturnsUser()
        {
            // Arrange
            var userEmail = "test@test.com";
            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                Email = userEmail
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(userEmail)).ReturnsAsync(user);

            var mockBus = new Mock<IBus>();
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            var actualUser = await service.GetAsync(userEmail);

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public async Task GetOrCreate_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var userEmail = "test@test.com";
            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                Email = userEmail
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(userEmail)).ReturnsAsync(user);

            var mockBus = new Mock<IBus>();
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            var actualUser = await service.GetOrCreateAsync(userEmail);

            // Assert
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public async Task GetOrCreate_WhenUserDoesntExist_ReturnsCreatedUserAndPublishesEvent()
        {
            // Arrange
            var userEmail = "test@test.com";
            var userId = Guid.NewGuid();
            var user = new Models.User
            {
                Id = userId,
                Email = userEmail
            };

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.CreateAsync(userEmail)).ReturnsAsync(user);

            UserCreatedEvent userCreatedEvent = null;

            var mockBus = new Mock<IBus>();
            mockBus.Setup(x => x.PublishAsync(It.IsAny<UserCreatedEvent>()))
                .Callback<UserCreatedEvent>(r => userCreatedEvent = r);
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            var actualUser = await service.GetOrCreateAsync(userEmail);

            // Assert
            Assert.Equal(user, actualUser);
            mockBus.Verify(x => x.PublishAsync(It.IsAny<UserCreatedEvent>()), Times.Once);
            Assert.Equal(userId, userCreatedEvent.Id);
        }


        [Fact]
        public async Task RedeemMatchRewards_AddsXpAndMoney()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Models.User
            {
                Id = userId,
                XP = 0,
                Money = 0
            };

            Models.User updatedUser = null;

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(userId)).ReturnsAsync(user);
            mockUserRepository.Setup(x => x.UpdateAsync(It.IsAny<Models.User>())).Callback<Models.User>(r => updatedUser = r);

            var mockBus = new Mock<IBus>();
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            await service.RedeemMatchRewards(userId, true);

            // Assert
            Assert.True(updatedUser.Money > 0);
            Assert.True(updatedUser.XP > 0);
        }

        [Fact]
        public async Task TakePayment_Calls_TakePayment()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockUserRepository = new Mock<IUserRepository>();

            var mockBus = new Mock<IBus>();
            var service = new UserService(mockUserRepository.Object, mockBus.Object);

            // Act
            await service.TakePayment(userId, 1);

            // Assert
            mockUserRepository.Verify(x => x.TakePayment(userId, 1), Times.Once);
        }
    }
}
