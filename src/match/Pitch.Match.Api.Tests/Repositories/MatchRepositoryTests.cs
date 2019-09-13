using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;
using Xunit;

namespace Pitch.Match.Api.Tests.Repositories
{
    public class MatchRepositoryTests
    {
        [Fact]
        public async Task GetAsync_MatchIdGuidPassed_ReturnsMatchWithId()
        {
            // Arrange
            var matchId = Guid.NewGuid();

            var stubMatch = new ApplicationCore.Models.Match
            {
                Id = matchId
            };

            var mockDataContext = new Mock<IDataContext<ApplicationCore.Models.Match>>();
            mockDataContext.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ApplicationCore.Models.Match, bool>>>())).Returns(Task.FromResult(stubMatch));
            
            // Act
            var repository = new MatchRepository(mockDataContext.Object);
            var result = await repository.GetAsync(matchId);

            //Assert
            Assert.Equal(stubMatch, result);
        }
    }
}
