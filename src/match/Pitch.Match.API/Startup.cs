using AutoMapper;
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
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Engine.Providers;
using Pitch.Match.API.Hubs;
using Pitch.Match.API.Infrastructure.MessageBus.Supporting;
using Pitch.Match.API.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.OpenApi.Models;
using Pitch.Match.API.ApplicationCore.Engine.Services;
using Pitch.Match.API.ApplicationCore.Services;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;
using Swashbuckle.AspNetCore.Swagger;

namespace Pitch.Match.API
{
    [ExcludeFromCodeCoverage]
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

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
            services.AddSingleton<IActionService, ActionService>();

            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped(typeof(IDataContext<>), typeof(MongoDbDataContext<>));

            services.AddSingleton<IRandomnessProvider, ThreadSafeRandomnessProvider>();
            services.AddSingleton<IAction, ApplicationCore.Engine.Actions.Foul>();
            services.AddSingleton<IAction, Shot>();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQHealthCheck"), name: "rabbitmq-check", tags: new string[] { "rabbitmq" })
                .AddMongoDb(Configuration.GetConnectionString("MongoDb"), name: "mongodb-check", tags: new string[] { "mongodb" });
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Match API", Version = "v1" });
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
            BsonClassMap.RegisterClassMap<ApplicationCore.Engine.Events.Foul>();
            BsonClassMap.RegisterClassMap<Substitution>();

            app.UseSwagger(c =>
            {
                //var basePath = "/match";
                //c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = basePath);

                //c.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                //    IDictionary<string, PathItem> paths = new Dictionary<string, PathItem>();
                //    foreach (var path in swaggerDoc.Paths)
                //    {
                //        paths.Add(path.Key.Replace(basePath, "/"), path.Value);
                //    }
                //    swaggerDoc.Paths = paths;
                //});
            });

            app.UseHealthChecks("/health");
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseAuthentication();

            app.UseSignalR(route =>
            {
                route.MapHub<MatchmakingHub>("/hubs/matchmaking", (options) =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransports.All;
                });
            });

            app.UseRouting();
            app.UseAuthorization();
        }
    }
}
