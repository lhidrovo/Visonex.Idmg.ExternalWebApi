using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Visonex.Idmg.ExternalWebApi.Services;

namespace Visonex.Idmg.ExternalWebApi.Middlewares
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IServiceCredentialsRepository _serviceCredentialRepository;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceCredentialsRepository serviceCredentialRepository)
            : base(options, logger, encoder, clock)
        {
            _serviceCredentialRepository = serviceCredentialRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header not found");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                if (await IsAuthenticatedAsync(username, password))
                {
                    var identity = new ClaimsIdentity("Basic");
                    identity.AddClaim(new Claim(ClaimTypes.Name, username));

                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid authorization header");
            }
        }

        private async Task<bool> IsAuthenticatedAsync(string username, string password)
        {
            var apiCredentials = await _serviceCredentialRepository.GetServiceCredentialsAsync(interfaceType: "IdMgExternalApi");
            
            return username == apiCredentials?.UserName && password == apiCredentials.Password;
        }
    }
}
