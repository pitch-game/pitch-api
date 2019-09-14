using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.Infrastructure.Repositories;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
using Xunit;

namespace Pitch.Match.API.Tests.Repositories
{
    public class MatchRepositoryTests
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
        }

        [Fact]
        public async Task GetAsync_MatchIdGuidPassed_ReturnsMatchWithId()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match
            {
                Id = matchId
            };
            var stubMatchList = new List<ApplicationCore.Models.Match>() {stubMatch};

            var dbContextStub = new InMemoryDataContext<ApplicationCore.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetAsync(matchId);

            //Assert
            Assert.Equal(stubMatch, result);
        }

        [Fact]
        public async Task GetInProgressAsync_WithMatchInProgressAsHomeTeam_ReturnsMatchId()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match
            {
                Id = matchId,
                HomeTeam = new TeamDetails()
                {
                    UserId = userId
                },
                AwayTeam =  new TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                KickOff = DateTime.Now.AddMinutes(-10)
            };
            var stubMatchList = new List<ApplicationCore.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<ApplicationCore.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetInProgressAsync(userId);

            //Assert
            Assert.Equal(stubMatch.Id, result);
        }

        [Fact]
        public async Task GetInProgressAsync_WithMatchInProgressAsAwayTeam_ReturnsMatchId()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match
            {
                Id = matchId,
                HomeTeam = new TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                AwayTeam = new TeamDetails()
                {
                    UserId = userId
                },
                KickOff = DateTime.Now.AddMinutes(-10)
            };
            var stubMatchList = new List<ApplicationCore.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<ApplicationCore.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetInProgressAsync(userId);

            //Assert
            Assert.Equal(stubMatch.Id, result);
        }

        [Fact]
        public async Task GetInProgressAsync_WithCompletedMatch_ReturnsNull()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match
            {
                Id = matchId,
                HomeTeam = new TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                AwayTeam = new TeamDetails()
                {
                    UserId = userId
                },
                KickOff = DateTime.Now.AddMinutes(-100)
            };
            var stubMatchList = new List<ApplicationCore.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<ApplicationCore.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetInProgressAsync(userId);

            //Assert
            Assert.Equal((Guid?)null, result);
        }

        [Fact]
        public async Task GetInProgressAsync_WithNoMatchInProgress_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stubMatchList = new List<ApplicationCore.Models.Match>();

            var dbContextStub = new InMemoryDataContext<ApplicationCore.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetInProgressAsync(userId);

            //Assert
            Assert.Equal((Guid?)null, result);
        }


        [Fact]
        public async Task CreateAsync_CallsCreateASyncOnDbContextOnce()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var stubMatch = new ApplicationCore.Models.Match()
            {
                Id = matchId
            };

            var dbContextMock = new Mock<IDataContext<ApplicationCore.Models.Match>>();
            dbContextMock.Setup(x => x.CreateAsync(stubMatch));

            // Act
            var repository = new MatchRepository(dbContextMock.Object);
            var result = await repository.CreateAsync(stubMatch);

            //Assert
            dbContextMock.Verify(x => x.CreateAsync(stubMatch), Times.Once);
        }
    }
}
