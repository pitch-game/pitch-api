namespace Pitch.Match.Api.ApplicationCore.Engine
{
    public interface IMatchEngine
    {
        Models.Match SimulateReentrant(Models.Match match);
    }
}