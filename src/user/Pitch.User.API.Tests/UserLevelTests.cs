using Xunit;

namespace Pitch.User.API.Tests
{
    public class UserLevelTests
    {
        [Fact]
        public void NoXp_ReturnsLevel1()
        {
            var userStub = new Models.User()
            {
                XP = 0
            };

            Assert.Equal(1, userStub.Level);
        }
    }
}
