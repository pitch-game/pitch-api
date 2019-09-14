using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Squad.API.Application.Responders;
using System.Collections.Generic;

namespace Pitch.Squad.API.Supporting
{
    public static class ApplicationBuilderExtentions
    {
        private static IEnumerable<IResponder> _responders { get; set; }

        public static IApplicationBuilder UseEasyNetQ(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                _responders = scope.ServiceProvider.GetServices<IResponder>();
            }

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            foreach (var responder in _responders)
            {
                responder.Register();
            }
        }

        private static void OnStopping() { }
    }
}
