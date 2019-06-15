using Pitch.Match.Api.Models;

namespace Pitch.Match.Api.Services
{
    public interface IMatchEngine
    {
        Models.Match SimulateReentrant(Models.Match match);
    }
}