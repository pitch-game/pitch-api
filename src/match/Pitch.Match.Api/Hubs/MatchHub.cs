using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Pitch.Match.Api.Hubs
{
    [Authorize]
    public class MatchHub : Hub
    {
        public void StreamEvents()
        {
            //Match kick off
            //Time now
            //Get Events up until now
            //Stream events
        }
    }
}
