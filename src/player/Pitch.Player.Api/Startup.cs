using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Pitch.Player.Api.Services;
using Pitch.Player.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using Pitch.Player.Api.Application.Responders;
using Pitch.Player.Api.Application.Requests;
using Pitch.Player.Api.Application.Responses;
using Pitch.Player.Api.Supporting;
using RabbitMQ.Client;

namespace Pitch.Player.Api
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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQHealthCheck"), name: "rabbitmq-check", tags: new string[] { "rabbitmq" });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IPlayerRequestResponder, PlayerRequestResponder>();
            services.AddSingleton<IResponder, PlayerRequestResponder>();

            services.AddSingleton<IPlayerService, PlayerService>();

            services.AddSingleton(s =>
            {
                return RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"), serviceRegister =>
                    serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new SimpleTypeNameSerializer()));
            });

            services.AddSingleton(sp =>
            {
                var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.GetData("AppDataDir").ToString(), "players_sample.json"));
                return JsonConvert.DeserializeObject<IList<Models.Player>>(json);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            string baseDir = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("AppDataDir", System.IO.Path.Combine(baseDir, "App_Data"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseEasyNetQ();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
