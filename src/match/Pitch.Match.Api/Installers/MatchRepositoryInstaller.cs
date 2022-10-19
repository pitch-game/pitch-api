using Microsoft.Extensions.DependencyInjection;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.Api.Installers
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
