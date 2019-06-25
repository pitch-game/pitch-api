using System;
using System.Threading;
using System.Threading.Tasks;
using PitchApi.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;
using EasyNetQ;
using Pitch.Identity.Api.Supporting;

namespace PitchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // http://localhost:5000/connect/authorize?client_id=cbf24cc4a1bb79e441a5b5937be6dd84&redirect_uri=https%3A%2F%2Foidcdebugger.com%2Fdebug&scope=openid&response_type=id_token&response_mode=fragment&nonce=tbgr049ja3

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkCosmos();
            services.AddHealthChecks();

            services.AddDbContext<AuthorizationDbContext>(options =>
            {
                options.UseInMemoryDatabase("pitch-im");
                options.UseOpenIddict();
            });

            services.AddMvc();

            services.AddSingleton(s =>
            {
                return RabbitHutch.CreateBus(Configuration.GetConnectionString("ServiceBus"), serviceRegister =>
                    serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new SimpleTypeNameSerializer()));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration.GetValue<string>("Authentication:Google:ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("Authentication:Google:ClientSecret");
                });

            services.AddOpenIddict()
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and entities.
                options.UseEntityFrameworkCore().UseDbContext<AuthorizationDbContext>();
            })
            .AddServer(options =>
            {
                options.UseMvc();
                options.EnableAuthorizationEndpoint("/connect/authorize");
                options.AllowImplicitFlow();
                options.DisableHttpsRequirement();
                options.AddEphemeralSigningKey();
                options.IgnoreEndpointPermissions();
                options.IgnoreGrantTypePermissions();
                options.IgnoreScopePermissions();
                options.SetIssuer(new Uri(Configuration.GetValue<string>("Issuer")));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks("/health");
            app.UseHealthChecks("/liveness");

            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };
            forwardingOptions.KnownNetworks.Clear(); //Loopback by default, this should be temporary
            forwardingOptions.KnownProxies.Clear(); //Update to include
            app.UseForwardedHeaders(forwardingOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                context.Request.PathBase = new PathString("/identity");
                return next();
            });

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {

            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>().Database.EnsureCreatedAsync();
                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("cbf24cc4a1bb79e441a5b5937be6dd84", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "cbf24cc4a1bb79e441a5b5937be6dd84",
                        DisplayName = "Angular Application",
                        PostLogoutRedirectUris = { new Uri("http://localhost:4200"), new Uri("http://pitch-game.io") },
                        RedirectUris = { new Uri("http://localhost:4200/auth-callback"), new Uri("http://pitch-game.io/auth-callback") }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }
    }
}
