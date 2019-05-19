using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IPlayerRequestResponder, PlayerRequestResponder>();
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

            app.UseRabbitListener();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
    public static class ApplicationBuilderExtentions
    {
        private static IBus _bus { get; set; }
        private static IPlayerRequestResponder _playerRequestResponder { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            _bus = app.ApplicationServices.GetService<IBus>();
            _playerRequestResponder = app.ApplicationServices.GetService<IPlayerRequestResponder>();

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            _bus.Respond<PlayerRequest, PlayerResponse>(_playerRequestResponder.Response); //todo move to each responder and register automatically
        }

        private static void OnStopping()
        {
            _bus.Dispose();
        }
    }
}
