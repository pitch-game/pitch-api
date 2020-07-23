using Microsoft.Extensions.DependencyInjection;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Engine.Providers;
using Pitch.Match.API.ApplicationCore.Engine.Services;

namespace Pitch.Match.API.Installers
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
            services.AddSingleton<IAction, ApplicationCore.Engine.Actions.Foul>();
            services.AddSingleton<IAction, Shot>();

            return services;
        }
    }
}
