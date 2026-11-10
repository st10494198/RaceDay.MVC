
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ResultApiService
    {
        private readonly HttpClient _httpClient;

        public ResultApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonElement?> GetMyResultsAsync(string token)
        {
            try
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Get, "api/results/my");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result =
                    await response.Content.ReadFromJsonAsync<JsonElement>();

                if (result.ValueKind != JsonValueKind.Array)
                {
                    return null;
                }

                return result;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
