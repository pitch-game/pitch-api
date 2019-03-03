using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using PitchApi.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using Pitch.Domain.Services;
using Pitch.DataStorage.Contexts;

namespace PitchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // http://localhost:5000/connect/authorize?client_id=angular-app&redirect_uri=https%3A%2F%2Foidcdebugger.com%2Fdebug&scope=openid&response_type=id_token&response_mode=fragment&nonce=tbgr049ja3

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkCosmos();

            services.AddDbContext<AuthorizationDbContext>(options =>
            {
                //Store this in memory for now
                options.UseInMemoryDatabase("pitch-im");
                //options.UseCosmos(Configuration["CosmosDb:EndpointURI"], Configuration["CosmosDb:PrivateKey"], "pitch");

                options.UseOpenIddict();
            });

            services.AddDbContext<PitchContext>(options =>
            {
                options.UseCosmos(Configuration["CosmosDb:EndpointURI"], Configuration["CosmosDb:PrivateKey"], "pitch");
            });

            services.AddCors();

            services.AddMvc();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["AppUri"];
                    options.Audience = "angular-app";
                    options.RequireHttpsMetadata = false;
                });

            services.AddOpenIddict()
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and entities.
                options.UseEntityFrameworkCore().UseDbContext<AuthorizationDbContext>();
            })
            .AddServer(options =>
            {
                // Register the ASP.NET Core MVC binder used by OpenIddict
                options.UseMvc();

                // Enable the authorization endpoints
                options.EnableAuthorizationEndpoint("/connect/authorize");

                // Allow client applications to use the implicit flow.
                options.AllowImplicitFlow();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                options.AddEphemeralSigningKey();

                options.IgnoreEndpointPermissions();
                options.IgnoreGrantTypePermissions();
                options.IgnoreScopePermissions();
            });

            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins(new string[] {"http://localhost:4200", "https://pitch-game.io"}).AllowAnyHeader().AllowCredentials());

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {

            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //await scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>().Database.EnsureCreatedAsync();
                await scope.ServiceProvider.GetRequiredService<PitchContext>().Database.EnsureCreatedAsync();
                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("angular-app", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "angular-app",
                        DisplayName = "Angular Application",
                        PostLogoutRedirectUris = { new Uri("http://localhost:4200"), new Uri("https://pitch-game.io") },
                        RedirectUris = { new Uri("http://localhost:4200/auth-callback"), new Uri("https://pitch-game.io/auth-callback") }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }
    }
}
