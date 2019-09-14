namespace Pitch.Match.API.ApplicationCore.Engine
{
    public interface IMatchEngine
    {
        Models.Match SimulateReentrant(Models.Match match);
    }
}