using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Store.API.Application.Subscribers;
using System.Collections.Generic;

namespace Pitch.User.API.Supporting
{
    public static class ApplicationBuilderExtentions
    {
        private static IEnumerable<ISubscriber> _subscribers { get; set; }

        public static IApplicationBuilder UseEasyNetQ(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                _subscribers = scope.ServiceProvider.GetServices<ISubscriber>();
            }
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Subscribe();
            }
        }

        private static void OnStopping() { }
    }
}