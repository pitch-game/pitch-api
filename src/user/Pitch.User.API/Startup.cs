using System;
using System.Linq;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pitch.User.API.Application.Responders;
using Pitch.User.API.Application.Subscribers;
using Pitch.User.API.Infrastructure.Repositories;
using Pitch.User.API.Infrastructure.Repositories.Contexts;
using Pitch.User.API.Services;
using Pitch.User.API.Supporting;
using Swashbuckle.AspNetCore.Swagger;

namespace Pitch.User.API
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IDataContext<>), typeof(MongoDbDataContext<>));

            services.AddScoped<IGetOrCreateUserResponder, GetOrCreateUserResponder>();
            services.AddScoped<IResponder, GetOrCreateUserResponder>();

            services.AddScoped<ISubscriber, MatchCompletedEventSubscriber>();

            services.AddScoped<IUserService, UserService>();

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

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQHealthCheck"), name: "rabbitmq-check", tags: new string[] { "rabbitmq" });

            services.AddSingleton(s =>
            {
                var typesInAssembly = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
                return RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"), serviceRegister =>
                    serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new SimpleTypeNameSerializer(typesInAssembly)));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "User API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseHealthChecks("/health");
            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseAuthentication();
            app.UseEasyNetQ();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
