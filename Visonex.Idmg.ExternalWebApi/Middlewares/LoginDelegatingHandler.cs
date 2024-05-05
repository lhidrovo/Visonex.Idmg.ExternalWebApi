using System.Net.Http.Headers;
using System.Text;
using Visonex.Idmg.ExternalWebApi.Common;
using Visonex.Idmg.ExternalWebApi.Dtos;
using Visonex.Idmg.ExternalWebApi.Services;

namespace Visonex.Idmg.ExternalWebApi.Middlewares
{
    public class LoginDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceCredentialsRepository _serviceCredentialsRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginDelegatingHandler(
            IServiceCredentialsRepository serviceCredentialsRepository,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceCredentialsRepository = serviceCredentialsRepository;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string tenant = string.Empty;

            string? organizationName = _httpContextAccessor.HttpContext?.Request?.Headers?
                .SingleOrDefault(t => t.Key == CustomHeaders.OrganizationNameHeader).Value.ToString();

            (clientId, clientSecret, tenant) = await _serviceCredentialsRepository
                .GetOrganizationCredentials(interfaceType: "GraphAPI", dataBaseName: organizationName!);

            var token = await GetAccessTokenAsync(clientId, clientSecret, tenant);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string?> GetAccessTokenAsync(string clientId, string clientSecret, string tenant)
        {
            string tokenEndpoint = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token";

            var requestBody = new StringBuilder();
            requestBody.Append($"client_id={clientId}");
            requestBody.Append($"&scope=https%3A%2F%2Fgraph.microsoft.com%2F.default");
            requestBody.Append($"&client_secret={clientSecret}");
            requestBody.Append("&grant_type=client_credentials");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(tokenEndpoint,
                new StringContent(requestBody.ToString(),
                Encoding.UTF8,
                "application/x-www-form-urlencoded"));

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var tokenResponse = await System.Text.Json.JsonSerializer.DeserializeAsync<TokenResponse>(stream);
                return tokenResponse?.AccessToken;
            }
            else
            {
                Console.WriteLine("Error occurred while retrieving access token: " + response.ReasonPhrase);
                return null;
            }
        }
    }
}
