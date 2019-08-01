namespace Pitch.Match.Api.Application.Engine
{
    public interface IMatchEngine
    {
        Models.Match SimulateReentrant(Models.Match match);
    }
}