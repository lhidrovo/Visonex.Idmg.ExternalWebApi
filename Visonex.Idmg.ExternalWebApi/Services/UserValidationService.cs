namespace Visonex.Idmg.ExternalWebApi.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IGraphApiService _graphApiService;

        public UserValidationService(IGraphApiService graphApiService)
        {
            _graphApiService = graphApiService;
        }

        public async Task<bool> ValidateUserExistsAsync(string userEmail)
        {
            var user = await _graphApiService.GetUserByEmailAsync(userEmail);
            return user != null;
        }
    }
}
