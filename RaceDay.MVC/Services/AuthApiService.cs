
using System.Net.Http.Json;
using System.Text.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(
            string email,
            string password)
        {
            var loginData = new
            {
                email,
                password
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                loginData
            );

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"API login failed: {(int)response.StatusCode} - {responseContent}"
                );
            }

            var result = JsonSerializer.Deserialize<LoginResponse>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return result;
        }
    }
}
