using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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

        [Fact]
        public async Task MatchmakingHub_MatchmakeMethodOnOpenSession_CallsOnceReceiveSessionId()
        {
            var matchServiceMock = new Mock<IMatchService>();
            var matchmakingServiceMock = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            matchmakingServiceMock.Setup(x => x.Matchmake(It.IsAny<Guid>())).Returns(Task.FromResult(new MatchmakingSession()
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                CreatedOn = DateTime.Now
            }));

            var groupManagerMock = new Mock<IGroupManager>();

            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.UserIdentifier).Returns(Guid.NewGuid().ToString);
            hubCallerContextMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            Mock<IMatchmakingClient> mockClientProxy = new Mock<IMatchmakingClient>();
            Mock<IHubCallerClients<IMatchmakingClient>> mockClients = new Mock<IHubCallerClients<IMatchmakingClient>>();
            mockClients.Setup(clients => clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var matchmakingHubStub = new MatchmakingHub(matchmakingServiceMock.Object, matchServiceMock.Object);
            matchmakingHubStub.Context = hubCallerContextMock.Object;
            matchmakingHubStub.Groups = groupManagerMock.Object;
            matchmakingHubStub.Clients = mockClients.Object;

            //Act
            await matchmakingHubStub.Matchmake();

            //Assert
            mockClients.Verify(x => x.User(It.IsAny<string>()).ReceiveSessionId(sessionId), Times.Once);
        }

        [Fact]
        public async Task MatchmakingHub_ValidateAndSubscribeMethod_CallsOnceGetSession()
        {
            var matchServiceMock = new Mock<IMatchService>();
            var matchmakingServiceMock = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            matchmakingServiceMock.Setup(x => x.GetSession(It.IsAny<Guid>())).Returns(new MatchmakingSession()
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                CreatedOn = DateTime.Now
            });

            var groupManagerMock = new Mock<IGroupManager>();

            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.UserIdentifier).Returns(Guid.NewGuid().ToString);
            hubCallerContextMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            Mock<IMatchmakingClient> mockClientProxy = new Mock<IMatchmakingClient>();
            Mock<IHubCallerClients<IMatchmakingClient>> mockClients = new Mock<IHubCallerClients<IMatchmakingClient>>();
            mockClients.Setup(clients => clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var matchmakingHubStub = new MatchmakingHub(matchmakingServiceMock.Object, matchServiceMock.Object);
            matchmakingHubStub.Context = hubCallerContextMock.Object;
            matchmakingHubStub.Groups = groupManagerMock.Object;
            matchmakingHubStub.Clients = mockClients.Object;

            //Act
            await matchmakingHubStub.ValidateAndSubscribe(sessionId.ToString());

            //Assert
            matchmakingServiceMock.Verify(x => x.GetSession(sessionId), Times.Once);
        }

        [Fact]
        public async Task MatchmakingHub_ValidateAndSubscribeMethodForClosedSession_CallsOnceCancelled()
        {
            var matchServiceMock = new Mock<IMatchService>();
            var matchmakingServiceMock = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            matchmakingServiceMock.Setup(x => x.GetSession(It.IsAny<Guid>())).Returns(new MatchmakingSession()
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                JoinedPlayerId = Guid.NewGuid()
            });

            var groupManagerMock = new Mock<IGroupManager>();

            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.UserIdentifier).Returns(Guid.NewGuid().ToString);
            hubCallerContextMock.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            Mock<IMatchmakingClient> mockClientProxy = new Mock<IMatchmakingClient>();
            Mock<IHubCallerClients<IMatchmakingClient>> mockClients = new Mock<IHubCallerClients<IMatchmakingClient>>();
            mockClients.Setup(clients => clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var matchmakingHubStub = new MatchmakingHub(matchmakingServiceMock.Object, matchServiceMock.Object);
            matchmakingHubStub.Context = hubCallerContextMock.Object;
            matchmakingHubStub.Groups = groupManagerMock.Object;
            matchmakingHubStub.Clients = mockClients.Object;

            //Act
            await matchmakingHubStub.ValidateAndSubscribe(sessionId.ToString());

            //Assert
            mockClients.Verify(x => x.User(It.IsAny<string>()).Cancelled(), Times.Once);
        }

        [Fact]
        public async Task MatchmakingHub_CancelMethod_CallsOnceCancel()
        {
            var matchServiceMock = new Mock<IMatchService>();
            var matchmakingServiceMock = new Mock<IMatchmakingService>();
            var sessionId = Guid.NewGuid();
            matchmakingServiceMock.Setup(x => x.GetSession(It.IsAny<Guid>())).Returns(new MatchmakingSession()
            {
                Id = sessionId,
                HostPlayerId = Guid.NewGuid(),
                CreatedOn = DateTime.Now
            });

            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.UserIdentifier).Returns(Guid.NewGuid().ToString);

            Mock<IMatchmakingClient> mockClientProxy = new Mock<IMatchmakingClient>();
            Mock<IHubCallerClients<IMatchmakingClient>> mockClients = new Mock<IHubCallerClients<IMatchmakingClient>>();
            mockClients.Setup(clients => clients.User(It.IsAny<string>())).Returns(mockClientProxy.Object);

            var matchmakingHubStub = new MatchmakingHub(matchmakingServiceMock.Object, matchServiceMock.Object);
            matchmakingHubStub.Context = hubCallerContextMock.Object;
            matchmakingHubStub.Clients = mockClients.Object;

            //Act
            await matchmakingHubStub.Cancel(sessionId.ToString());

            //Assert
            matchmakingServiceMock.Verify(x => x.Cancel(sessionId), Times.Once);
        }
    }
}
