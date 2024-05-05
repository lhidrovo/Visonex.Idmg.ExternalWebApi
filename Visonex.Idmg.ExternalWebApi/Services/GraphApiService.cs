using Visonex.Idmg.ExternalWebApi.Dtos;

namespace Visonex.Idmg.ExternalWebApi.Services
{
    public class GraphApiService : IGraphApiService
    {
        private readonly HttpClient _httpClient;

        public GraphApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/users");
        }

        public async Task<UserInfo?> GetUserByEmailAsync(string userEmail)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"?$filter=mail+eq+'{userEmail}'");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error occurred while calling the API: " + response.ReasonPhrase);
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            var userEntrResponse = System.Text.Json.JsonSerializer.Deserialize<UserEntraResponse>(content);

            if (userEntrResponse?.value?.Length <= 0)
            {
                return null;
            }

            return userEntrResponse?.value?.FirstOrDefault();
        }
    }
}
