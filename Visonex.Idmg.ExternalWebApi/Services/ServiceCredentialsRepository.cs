using Visonex.Idmg.ExternalWebApi.Dtos;

namespace Visonex.Idmg.ExternalWebApi.Services
{
    public class ServiceCredentialsRepository : IServiceCredentialsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceCredentialsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(string clientId, string clientSecret, string tenant)> GetOrganizationCredentials(string interfaceType, string dataBaseName)
        {
            //call service credentials API
            //var client = _httpClientFactory.CreateClient(HttpClientName.ServiceCredentials);

            //var credential = await client.GetFromJsonAsync<Credential?>($"/credentials?interfaceType={interfaceType}&database={dataBaseName}");

            //return credential;

            var credentials = new Credential
            {
                RequesterId = "b8fb83b8-19d0-4572-825c-d8a92943a7e0", //tenantId
                UserName = "3fd74244-b8eb-480b-88c7-7e93bc547c4d", //clientId
                Password = "ZDu8Q~qRZMTQPWsOJJvN7rs1MKElPQgWgvd2Pdp2" //clientSecret
            };

            return await Task.FromResult((credentials.UserName, credentials.Password, credentials.RequesterId));
        }

        public async Task<Credential?> GetServiceCredentialsAsync(string interfaceType)
        {
            //call service credentials API
            //var client = _httpClientFactory.CreateClient(HttpClientName.ServiceCredentials);

            //var credential = await client.GetFromJsonAsync<Credential?>($"/credentials?interfaceType={interfaceType}");

            //return credential;

            return await Task.FromResult(new Credential { UserName = "admin", Password = "password" });
        }
    }
}
