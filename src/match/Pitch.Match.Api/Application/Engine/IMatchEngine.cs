using Pitch.Match.Api.Models;

namespace Pitch.Match.Api.Application.Engine
{
    public interface IMatchEngine
    {
        Models.Match SimulateReentrant(Models.Match match);
    }
}