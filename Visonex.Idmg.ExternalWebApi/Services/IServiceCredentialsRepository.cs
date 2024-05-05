using Visonex.Idmg.ExternalWebApi.Dtos;

namespace Visonex.Idmg.ExternalWebApi.Services
{
    public interface IServiceCredentialsRepository
    {
        Task<(string clientId, string clientSecret, string tenant)> GetOrganizationCredentials(string interfaceType, string dataBaseName);

        Task<Credential?> GetServiceCredentialsAsync(string interfaceType);
    }
}
