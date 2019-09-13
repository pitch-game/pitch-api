﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;
using Xunit;

namespace Pitch.Match.Api.Tests
{
    public class MongoDbDataContextTests
    {
        public class TestEntity : IEntity
        {
            public Guid Id { get; set; }
        }

        [Fact]
        public async Task CreateAsync_TestEntity_MongoInsertOneASyncIsCalled()
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
    }
}
