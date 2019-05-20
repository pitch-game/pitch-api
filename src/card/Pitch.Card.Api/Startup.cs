using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure;
using Pitch.Card.Api.Infrastructure.Handlers;
using Pitch.Card.Api.Infrastructure.Repositories;
using Pitch.Card.Api.Infrastructure.Requests;
using Pitch.Card.Api.Infrastructure.Services;
using Pitch.Card.Api.Supporting;

namespace Pitch.Card.Api
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

            var identityUrl = "https://localhost:44383/identity";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "cbf24cc4a1bb79e441a5b5937be6dd84";
            });

            //TODO Scoped
            services.AddSingleton<ICardRepository, CardRepository>();
            services.AddSingleton<ICreateCardResponder, CreateCardResponder>();
            services.AddSingleton<ICardService, CardService>();

            services.AddSingleton(s =>
            {
                return RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"), serviceRegister =>
                    serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new SimpleTypeNameSerializer()));
            });

            //services.AddSingleton(s =>
            //{
            //    var bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"));
            //    var sp = services.BuildServiceProvider();
            //    var createCardResponder = sp.GetService<ICreateCardResponder>();
            //    bus.Respond<CreateCardRequest, CreateCardResponse>(createCardResponder.Response); //todo move to each responder and register automatically
            //    return bus;
            //});

            services.AddDbContext<CardDbContext>(options => options.UseInMemoryDatabase(databaseName: "Cards"), ServiceLifetime.Singleton); //todo for now
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

            app.UseRabbitListener();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class ApplicationBuilderExtentions
    {
        private static IBus _bus { get; set; }
        private static ICreateCardResponder _createCardResponder { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            _bus = app.ApplicationServices.GetService<IBus>();
            _createCardResponder = app.ApplicationServices.GetService<ICreateCardResponder>();

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            _bus.RespondAsync<CreateCardRequest, CreateCardResponse>(_createCardResponder.Response); //todo move to each responder and register automatically
        }

        private static void OnStopping()
        {
            _bus.Dispose();
        }
    }
}
