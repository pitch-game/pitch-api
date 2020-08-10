using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Pitch.Squad.API.Infrastructure;
using Xunit;

namespace Pitch.Squad.API.Tests.Contexts
{
    public class MongoDbDataContextTests
    {
        public class TestEntity : IEntity
        {
            public Guid Id { get; set; }

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

                var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
                await mongoDbContext.CreateAsync(testEntity);

                mongoCollectionMock.Verify(m => m.InsertOneAsync(testEntity, null, It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task DeleteAsync_TestEntity_CalledOnceMongoDeleteOneAsync()
        {
            var testId = Guid.NewGuid();

            var mongoCollectionMock = new Mock<IMongoCollection<TestEntity>>();
            mongoCollectionMock.Setup(x => x.InsertOneAsync(It.IsAny<TestEntity>(), null, It.IsAny<CancellationToken>()));

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(x => x.GetCollection<TestEntity>(It.IsAny<string>(), null)).Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            await mongoDbContext.DeleteAsync(x => x.Id == testId);

            mongoCollectionMock.Verify(m => m.DeleteOneAsync(It.IsAny<FilterDefinition<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
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

            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            await mongoDbContext.CreateAsync(testEntity);

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

            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            await mongoDbContext.UpdateAsync(testEntity);

            mongoCollectionMock.Verify(m => m.ReplaceOneAsync(It.IsAny<FilterDefinition<TestEntity>>(), testEntity, (ReplaceOptions)null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Return_Single_Object_When_One_Match_Exists()
        {
            var expectedGuid = new Guid();

            var _data = new List<TestEntity>()
            {
                new TestEntity()
                {
                    Id = expectedGuid
                },
                new TestEntity()
                {
                    Id = Guid.NewGuid()
                }
            };

            var asyncCursor = new MockAsyncCursor<TestEntity>(_data);

            var mongoCollectionMock = new Mock<IMongoCollection<TestEntity>>();
            mongoCollectionMock.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<TestEntity>>(), It.IsAny<FindOptions<TestEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(asyncCursor);

            Expression<Func<TestEntity, bool>> MatchOnIdQuery()
            {
                return entity => entity.Id == expectedGuid;
            };

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(x => x.GetCollection<TestEntity>(It.IsAny<string>(), null)).Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            var result = await mongoDbContext.FindOneAsync(MatchOnIdQuery());

            result.Id.Should().Be(expectedGuid);
        }

        [Fact]
        public async Task Return_All_Objects()
        {
            var expectedGuid = new Guid();

            var _data = new List<TestEntity>()
            {
                new TestEntity()
                {
                    Id = expectedGuid
                },
                new TestEntity()
                {
                    Id = Guid.NewGuid()
                }
            };

            var asyncCursor = new MockAsyncCursor<TestEntity>(_data);

            var mongoCollectionMock = new Mock<IMongoCollection<TestEntity>>();
            mongoCollectionMock.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<TestEntity>>(), It.IsAny<FindOptions<TestEntity>>(), It.IsAny<CancellationToken>())).ReturnsAsync(asyncCursor);

            Expression<Func<TestEntity, bool>> MatchOnIdQuery()
            {
                return entity => entity.Id == expectedGuid;
            };

            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(x => x.GetCollection<TestEntity>(It.IsAny<string>(), null)).Returns(mongoCollectionMock.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(mongoDatabaseMock.Object);

            var mongoDbContext = new MongoDbDataContext<TestEntity>(mongoClientMock.Object);
            var results = await mongoDbContext.FindAsync(MatchOnIdQuery());

            results.Should().NotBeNullOrEmpty();
            results.Count().Should().Be(2);
        }
    }

    public class MockAsyncCursor<T> : IAsyncCursor<T>
    {
        private bool _called;

        public MockAsyncCursor(IEnumerable<T> items)
        {
            Current = items ?? Enumerable.Empty<T>();
        }

        public IEnumerable<T> Current { get; }

        public bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
        {
            return !_called && (_called = true);
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(MoveNext(cancellationToken));
        }

        public void Dispose()
        {
        }
    }

}
