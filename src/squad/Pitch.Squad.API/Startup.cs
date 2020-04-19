using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Pitch.Squad.API.Application.Responders;
using Pitch.Squad.API.Infrastructure;
using Pitch.Squad.API.Infrastructure.Repositories;
using Pitch.Squad.API.Services;
using Pitch.Squad.API.Supporting;
using Swashbuckle.AspNetCore.Swagger;

namespace Pitch.Squad.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHealthChecks();

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

            services.AddScoped<ISquadService, SquadService>();
            services.AddScoped<IChemistryService, ChemistryService>();

            services.AddScoped<ISquadRepository, SquadRepository>();
            services.AddScoped<ISquadValidationService, SquadValidationService>();

            services.AddScoped<IGetSquadResponder, GetSquadResponder>();

            services.AddScoped<IResponder, GetSquadResponder>();

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Squad API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseHealthChecks("/health");
            app.UseHealthChecks("/liveness");

            app.UseRouting();

            app.UseEasyNetQ();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
