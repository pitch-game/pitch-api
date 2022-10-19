using Microsoft.Extensions.DependencyInjection;
using Pitch.Match.Engine;
using Pitch.Match.Engine.Actions;
using Pitch.Match.Engine.Providers;
using Pitch.Match.Engine.Services;

namespace Pitch.Match.Api.Installers
{
    public static class MatchEngineInstaller
    {
        public static IServiceCollection AddMatchEngine(this IServiceCollection services)
        {
            services.AddSingleton<IMatchEngine, MatchEngine>();
            services.AddSingleton<IActionService, ActionService>();
            services.AddSingleton<ICalculatedCardStatService, CalculatedCardStatService>();
            services.AddSingleton<IRatingService, RatingService>();
            services.AddSingleton<IPossessionService, PossessionService>();
            services.AddSingleton<IFitnessDrainService, FitnessDrainService>();

            services.AddSingleton<IRandomnessProvider, ThreadSafeRandomnessProvider>();
            services.AddSingleton<IAction, Foul>();
            services.AddSingleton<IAction, Shot>();

            return services;
        }
    }
}
