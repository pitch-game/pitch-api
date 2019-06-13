using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Pitch.Identity.Api.Application.Requests;
using Pitch.Identity.Api.Application.Responses;

namespace AuthorizationServer.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IBus _bus;
        public AuthorizationController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet("~/connect/authorize")]
        public async Task<IActionResult> Authorize(OpenIdConnectRequest request)
        {
            if (request == null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            Debug.Assert(request.IsAuthorizationRequest(),
                "The OpenIddict binder for ASP.NET Core MVC is not registered. " +
                "Make sure services.AddOpenIddict().AddMvcBinders() is correctly called.");

            // Check if a user is authenticated. If not, challenge the GitHub authentication handler
            if (!User.Identity.IsAuthenticated)
                return Challenge("Google");

            var response = await _bus.RequestAsync<GetOrCreateUserRequest, GetOrCreateUserResponse>(new GetOrCreateUserRequest(User.FindFirstValue(ClaimTypes.Email)));
            var userId = response.Id;

            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity("OpenIddict");
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject, userId.ToString(),
                OpenIdConnectConstants.Destinations.AccessToken, OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, User.FindFirstValue(ClaimTypes.Name),
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Email, User.FindFirstValue(ClaimTypes.Email),
                OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.EmailVerified, "true",
                OpenIdConnectConstants.Destinations.IdentityToken); // We'll assume email is verified since we get it from GitHub
            var principal = new ClaimsPrincipal(identity);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal,
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);

            ticket.SetIdentityTokenLifetime(TimeSpan.FromDays(1));

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
    }
}