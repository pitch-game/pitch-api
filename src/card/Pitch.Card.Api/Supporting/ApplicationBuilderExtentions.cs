using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Card.API.Application.Responders;
using Pitch.Card.API.Application.Subscribers;
using System.Collections.Generic;

namespace Pitch.Player.API.Supporting
{
    public static class ApplicationBuilderExtentions
    {
        private static IEnumerable<IResponder> _responders { get; set; }
        private static IEnumerable<ISubscriber> _subscribers { get; set; }

        public static IApplicationBuilder UseEasyNetQ(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                _responders = scope.ServiceProvider.GetServices<IResponder>();
                _subscribers = scope.ServiceProvider.GetServices<ISubscriber>();
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
            foreach (var subscriber in _subscribers)
            {
                subscriber.Subscribe();
            }
        }

        private static void OnStopping() { }
    }
}
