
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonElement> GetEventsAsync()
        {
            var events = await _httpClient.GetFromJsonAsync<JsonElement>("api/events");

            return events;
        }
    }
}
