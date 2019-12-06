using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Pitch.Store.API.Infrastructure;
using Xunit;

namespace Pitch.Store.API.Tests.Contexts
{
    public class MongoDbDataContextTests
    {
        public class TestEntity : IEntity
        {
            public Guid Id { get; set; }
        }

        [Fact]
        public async Task CreateAsync_TestEntity_CalledOnceMongoInsertOneAsync()
        {
            var testId = Guid.NewGuid();
            var testEntity = new TestEntity()
            {
                Id = testId
            };

            var mongoCollectionMock = new Mock<IMongoCollection<TestEntity>>();
            mongoCollectionMock.Setup(x => x.InsertOneAsync(It.IsAny<TestEntity>(), null, It.IsAny<CancellationToken>()));

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(x => x.GetCollection<TestEntity>(It.IsAny<string>(), null)).Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            // Act
            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            await mongoDbContext.CreateAsync(testEntity);

            //Assert
            mongoCollectionMock.Verify(m => m.InsertOneAsync(testEntity, null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_TestEntity_CalledOnceMongoReplaceOneAsync()
        {
            var testId = Guid.NewGuid();
            var testEntity = new TestEntity()
            {
                Id = testId
            };

            var mongoCollectionMock = new Mock<IMongoCollection<TestEntity>>();
            mongoCollectionMock.Setup(x => x.InsertOneAsync(It.IsAny<TestEntity>(), null, It.IsAny<CancellationToken>()));

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(x => x.GetCollection<TestEntity>(It.IsAny<string>(), null)).Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            // Act
            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            await mongoDbContext.UpdateAsync(testEntity);

            //Assert
            mongoCollectionMock.Verify(m => m.ReplaceOneAsync(It.IsAny<FilterDefinition<TestEntity>>(), testEntity, null, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
