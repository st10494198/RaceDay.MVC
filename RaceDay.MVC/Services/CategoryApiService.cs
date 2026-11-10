
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonElement?> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            return result;
        }
    }
}
