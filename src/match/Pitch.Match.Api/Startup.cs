using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Pitch.Match.Api.Application.Engine;
using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Application.Engine.Events;
using Pitch.Match.Api.Hubs;
using Pitch.Match.Api.Infrastructure.Repositories;
using Pitch.Match.Api.Services;
using Pitch.Match.Api.Supporting;
using System;
using System.Linq;

namespace Pitch.Match.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityUrl");
                options.Audience = "cbf24cc4a1bb79e441a5b5937be6dd84";
                options.RequireHttpsMetadata = false;
            });

            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchmakingService, MatchmakingService>();
            services.AddSingleton<IMatchSessionService, MatchSessionService>();

            services.AddSingleton<IMatchEngine, MatchEngine>();

            services.AddScoped<IMatchRepository, MatchRepository>();

            services.AddSingleton<IAction, Application.Engine.Action.Foul>();
            services.AddSingleton<IAction, Shot>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQHealthCheck"), name: "rabbitmq-check", tags: new string[] { "rabbitmq" })
                .AddMongoDb(Configuration.GetConnectionString("MongoDb"), name: "mongodb-check", tags: new string[] { "mongodb" })
                //.AddSignalRHub("/hubs/matchmaking", name: "signalr-check", tags: new string[] { "signalr" });

            services.AddSingleton<IMongoClient>(s =>
            {
                return new MongoClient(Configuration.GetConnectionString("MongoDb"));
            });

            services.AddSingleton(s =>
            {
                var typesInAssembly = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
                return RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"), serviceRegister =>
                    serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new SimpleTypeNameSerializer(typesInAssembly)));
            });

            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            BsonClassMap.RegisterClassMap<RedCard>();
            BsonClassMap.RegisterClassMap<YellowCard>();
            BsonClassMap.RegisterClassMap<Goal>();
            BsonClassMap.RegisterClassMap<ShotOnTarget>();
            BsonClassMap.RegisterClassMap<ShotOffTarget>();
            BsonClassMap.RegisterClassMap<Application.Engine.Events.Foul>();
            BsonClassMap.RegisterClassMap<Substitution>();

            app.UseHealthChecks("/health");
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseAuthentication();

            app.UseSignalR(route =>
            {
                route.MapHub<MatchmakingHub>("/hubs/matchmaking", (options) => {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransports.All;
                });
            });

            app.UseMvc();
        }
    }
}
