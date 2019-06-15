using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pitch.Match.Api.Services;
using System;
using System.Threading.Tasks;

namespace Pitch.Match.Api.Hubs
{
    public interface IMatchmakingClient
    {
        Task ReceiveSessionId(Guid sessionId);
        Task MatchReady(Guid sessionId);
    }

    [Authorize]
    public class MatchmakingHub : Hub<IMatchmakingClient>
    {
        private readonly IMatchmakingService _matchmakingService;
        public MatchmakingHub(IMatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        public async Task Matchmake()
        {
            var user = Context.UserIdentifier;

            var session = _matchmakingService.Matchmake(new Guid(user));
            await Groups.AddToGroupAsync(Context.ConnectionId, session.Id.ToString());

            if (session.Open)
            {
                await Clients.User(user).ReceiveSessionId(session.Id);
            }
            else
            {
                await Clients.Group(session.Id.ToString()).MatchReady(session.Id);
            }
        }

        public void Cancel(Guid sessionId)
        {
            _matchmakingService.Cancel(sessionId);
        }
    }
}
