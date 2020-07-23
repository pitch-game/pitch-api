using Microsoft.Extensions.DependencyInjection;
using Pitch.Match.API.Infrastructure.Repositories;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.Installers
{
    public static class MatchRepositoryInstaller
    {
        public static IServiceCollection AddMatchRepository(this IServiceCollection services)
        {
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped(typeof(IDataContext<>), typeof(MongoDbDataContext<>));

            return services;
        }
    }
}
