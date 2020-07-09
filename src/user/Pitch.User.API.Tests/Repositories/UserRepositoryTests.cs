using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Pitch.User.API.Infrastructure.Repositories;
using Pitch.User.API.Infrastructure.Repositories.Contexts;
using Xunit;

namespace Pitch.User.API.Tests.Repositories
{
    public class UserRepositoryTests
    {
        public class InMemoryDataContext<T> : IDataContext<T> where T : IEntity
        {
            public InMemoryDataContext(List<T> inMemoryList = null)
            {
                InMemoryList = inMemoryList ?? new List<T>();
            }

            private IList<T> InMemoryList { get; set; }

            public Task CreateAsync(T item)
            {
                InMemoryList.Add(item);
                return Task.CompletedTask;
            }

            public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> query)
            {
                return Task.FromResult(InMemoryList.FirstOrDefault(query.Compile()));
            }

            public Task<IEnumerable<T>> ToListAsync(Expression<Func<T, bool>> query)
            {
                return Task.FromResult(InMemoryList.Where(query.Compile()));
            }

            public Task UpdateAsync(T item)
            {
                throw new NotImplementedException();
            }

            public Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public async Task UpdateAsync_CallsUpdateAsyncOnce()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stubUser = new Models.User
            {
                Id = userId
            };

            var dbContextMock = new Mock<IDataContext<Models.User>>();
            dbContextMock.Setup(x => x.CreateAsync(stubUser));

            // Act
            var repository = new UserRepository(dbContextMock.Object);
            await repository.UpdateAsync(stubUser);

            //Assert
            dbContextMock.Verify(x => x.UpdateAsync(stubUser), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_CallsCreateAsyncOnce()
        {
            // Arrange
            var email = "test@test.com";

            var dbContextMock = new Mock<IDataContext<Models.User>>();

            // Act
            var repository = new UserRepository(dbContextMock.Object);
            await repository.CreateAsync(email);

            //Assert
            dbContextMock.Verify(x => x.CreateAsync(It.IsAny<Models.User>()), Times.Once);
        }

        [Fact]
        public async Task GetAsyncByEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@test.com";
            var stubUser = new Models.User()
            {
                Id = Guid.NewGuid(),
                Email = email
            };
            var stubUserList = new List<Models.User>() {stubUser};

            var dbContextStub = new InMemoryDataContext<Models.User>(stubUserList);

            // Act
            var repository = new UserRepository(dbContextStub);
            var result = await repository.GetAsync(email);

            //Assert
            Assert.Equal(stubUser, result);
        }


        [Fact]
        public async Task GetAsyncById_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stubUser = new Models.User()
            {
                Id = userId,
            };
            var stubUserList = new List<Models.User>() { stubUser };

            var dbContextStub = new InMemoryDataContext<Models.User>(stubUserList);

            // Act
            var repository = new UserRepository(dbContextStub);
            var result = await repository.GetAsync(userId);

            //Assert
            Assert.Equal(stubUser, result);
        }
    }
}
