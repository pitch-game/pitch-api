using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Store.API.Application.Subscribers;

namespace Pitch.Store.API.Supporting
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtentions
    {
        private static IEnumerable<ISubscriber> Subscribers { get; set; }

        public static IApplicationBuilder UseEasyNetQ(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                Subscribers = scope.ServiceProvider.GetServices<ISubscriber>();
            }
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.Subscribe();
            }
        }

        private static void OnStopping() { }
    }
}