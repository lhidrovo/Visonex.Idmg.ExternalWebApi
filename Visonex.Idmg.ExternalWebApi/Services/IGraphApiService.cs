using Visonex.Idmg.ExternalWebApi.Dtos;

namespace Visonex.Idmg.ExternalWebApi.Services
{
    public interface IGraphApiService
    {
        Task<UserInfo?> GetUserByEmailAsync(string userEmail);
    }
}
