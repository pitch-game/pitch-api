﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;
using Pitch.Match.Engine.Models;
using Xunit;

namespace Pitch.Match.Api.Tests.Unit.Repositories
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

            public Task<T> FindOneAsync(Expression<Func<T, bool>> query)
            {
                return Task.FromResult(InMemoryList.FirstOrDefault(query.Compile()));
            }

            public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> query)
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
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() {stubMatch};

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

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
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId
                },
                AwayTeam =  new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                KickOff = DateTime.Now.AddMinutes(-10)
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

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
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId
                },
                KickOff = DateTime.Now.AddMinutes(-10)
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

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
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId
                },
                KickOff = DateTime.Now.AddMinutes(-100)
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

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
            var stubMatchList = new List<Infrastructure.Models.Match>();

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

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
            var stubMatch = new Infrastructure.Models.Match()
            {
                Id = matchId
            };

            var dbContextMock = new Mock<IDataContext<Infrastructure.Models.Match>>();
            dbContextMock.Setup(x => x.CreateAsync(stubMatch));

            // Act
            var repository = new MatchRepository(dbContextMock.Object);
            var result = await repository.CreateAsync(stubMatch);

            //Assert
            dbContextMock.Verify(x => x.CreateAsync(stubMatch), Times.Once);
        }

        [Fact]
        public async Task HasUnclaimedAsync_WhenUserHasUnclaimedMatchRewards_ReturnsTrue()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId,
                    HasClaimedRewards = false
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                KickOff = DateTime.Now.AddMinutes(-100)
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.HasUnclaimedAsync(userId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetUnclaimedAsync_WhenUserHasAnUnclaimedMatch_ReturnsUnclaimedMatch()
        {
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId,
                    HasClaimedRewards = false
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                KickOff = DateTime.Now.AddMinutes(-100)
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetUnclaimedAsync(userId);

            //Assert
            Assert.Equal(stubMatch,result.First());
        }

        [Fact]
        public async Task UpdateAsync_CallsUpdateAsyncOnce()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var stubMatch = new Infrastructure.Models.Match()
            {
                Id = matchId
            };

            var dbContextMock = new Mock<IDataContext<Infrastructure.Models.Match>>();
            dbContextMock.Setup(x => x.CreateAsync(stubMatch));

            // Act
            var repository = new MatchRepository(dbContextMock.Object);
            var result = await repository.UpdateAsync(stubMatch);

            //Assert
            dbContextMock.Verify(x => x.UpdateAsync(stubMatch), Times.Once);
        }

        [Fact]
        public async Task GetAllSync_WithOneMatchPlayed_ReturnsOneMatch()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = matchId,
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = userId,
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                }
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetAllAsync(0, 1, userId);

            //Assert
            Assert.Equal(stubMatch, result.First());
        }

        [Fact]
        public async Task GetAllSync_WithMatchNotInvolvingUser_ReturnsEmptyList()
        {
            // Arrange
            var stubMatch = new Infrastructure.Models.Match
            {
                Id = Guid.NewGuid(),
                HomeTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                },
                AwayTeam = new Infrastructure.Models.TeamDetails()
                {
                    UserId = Guid.NewGuid()
                }
            };
            var stubMatchList = new List<Infrastructure.Models.Match>() { stubMatch };

            var dbContextStub = new InMemoryDataContext<Infrastructure.Models.Match>(stubMatchList);

            // Act
            var repository = new MatchRepository(dbContextStub);
            var result = await repository.GetAllAsync(0, 1, Guid.NewGuid());

            //Assert
            Assert.False(result.Any());
        }
    }
}
