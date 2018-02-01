using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using System;
using mejor_precio_2.API;

namespace mejor_precio_2.ApiRest
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Name = "Custom";
        public CustomAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var header = this.Context.Request.Headers["X-CustomAuth"];

            if (header == StringValues.Empty)
            {
                return AuthenticateResult.NoResult();
            }

            var tokenStr = header.First();
            Guid token = new Guid(tokenStr);

            ClaimsPrincipal principal = ObtenerPrincipalPorToken(token);

            if (principal == null)
            {
                return AuthenticateResult.Fail("Token invalido");
            }

            var ticket = new AuthenticationTicket(principal, Name);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsPrincipal ObtenerPrincipalPorToken(Guid token)
        {
            var users = new UserHandler().getUsersTokens();
            foreach (var user in users)
            {
                if (token == user.Token)
                {
                    var usernameClaim = new Claim(ClaimTypes.Name, user.Name);
                    var roleClaim = new Claim(ClaimTypes.Role, user.Type);
                    var identity = new ClaimsIdentity(new[] { usernameClaim, roleClaim }, "token");
                    var principal = new ClaimsPrincipal(identity);

                    return principal;
                }
            }

            return null;
        }
    }
}