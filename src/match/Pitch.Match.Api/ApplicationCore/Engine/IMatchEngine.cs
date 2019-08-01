namespace Pitch.Match.Api.ApplicationCore.Engine
{
    public interface IMatchEngine
    {
        Models.Match.Match SimulateReentrant(Models.Match.Match match);
    }
}