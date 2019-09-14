using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Pitch.Match.API.ApplicationCore.Models.Matchmaking;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Hubs;
using Xunit;

namespace Pitch.Match.API.Tests.Hubs
{
    public class MatchmakingHubTests
    {
        [Fact]
        public async Task MatchmakingHub_MatchmakeMethod_CallsOnceMatchmake()
        {
            var matchServiceMock = new Mock<IMatchService>();
            var matchmakingServiceMock = new Mock<IMatchmakingService>();
            matchmakingServiceMock.Setup(x => x.Matchmake(It.IsAny<Guid>())).Returns(Task.FromResult(new MatchmakingSession()
            {
                Id = Guid.NewGuid(),
                HostPlayerId = Guid.NewGuid(),
                JoinedPlayerId = Guid.NewGuid()
            }));

            var groupManagerMock = new Mock<IGroupManager>();

            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.UserIdentifier).Returns(Guid.NewGuid().ToString);
            hubCallerContextMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            Mock<IMatchmakingClient> mockClientProxy = new Mock<IMatchmakingClient>();
            Mock<IHubCallerClients<IMatchmakingClient>> mockClients = new Mock<IHubCallerClients<IMatchmakingClient>>();
            mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var matchmakingHubStub = new MatchmakingHub(matchmakingServiceMock.Object, matchServiceMock.Object);
            matchmakingHubStub.Context = hubCallerContextMock.Object;
            matchmakingHubStub.Groups = groupManagerMock.Object;
            matchmakingHubStub.Clients = mockClients.Object;

            //Act
            await matchmakingHubStub.Matchmake();

            //Assert
            matchmakingServiceMock.Verify(x => x.Matchmake(It.IsAny<Guid>()), Times.Once);
        }
    }
}
