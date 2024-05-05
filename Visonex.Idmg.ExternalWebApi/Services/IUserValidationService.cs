namespace Visonex.Idmg.ExternalWebApi.Services
{
    public interface IUserValidationService
    {
        Task<bool> ValidateUserExistsAsync(string userEmail);
    }
}
