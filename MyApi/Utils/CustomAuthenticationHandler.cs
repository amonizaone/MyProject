using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApi.Data.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MyApi.Utils
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            )
            : base(options, logger, encoder, clock)
        {
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }
            if (!authorizationHeader.StartsWith(CustomAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            string token = authorizationHeader.Substring(CustomAuthenticationDefaults.AuthenticationScheme.Length).Trim();
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            try
            {
                return await ValidateTokenAsync(token);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }
        private async Task<AuthenticateResult> ValidateTokenAsync(string session)
        {
            // getting user info using HTTP request made using Flurl
            // var user? = null;
                //await Options.UserInfoEndpoint
                //.WithHeader("some-id", session)
                //.GetJsonAsync<User>();
            if (1 == 2)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                //new Claim(ClaimTypes.GivenName, $"{user.Name}"),
                //new Claim(ClaimTypes.Surname, surname),
                //new Claim("scope", "orders:write"),
                new Claim(ClaimTypes.NameIdentifier, "1")
                //new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
