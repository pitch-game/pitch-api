using Microsoft.Extensions.DependencyInjection;
using Pitch.Match.Api.ApplicationCore.Services;

namespace Pitch.Match.Api.Installers
{
    public static class ServicesInstaller
    {
        public static IServiceCollection AddMatchServices(this IServiceCollection services)
        {
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchmakingService, MatchmakingService>();
            services.AddSingleton<IMatchSessionService, MatchSessionService>();

            return services;
        }
    }
}
