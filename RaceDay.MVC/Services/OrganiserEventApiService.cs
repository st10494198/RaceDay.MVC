
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class OrganiserEventApiService
    {
        private readonly HttpClient _httpClient;

        public string? LastError { get; private set; }

        public OrganiserEventApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // View all events.
        public async Task<JsonElement?> GetEventsAsync()
        {
            try
            {
                var result = await _httpClient
                    .GetFromJsonAsync<JsonElement>("api/events");

                return result.ValueKind == JsonValueKind.Array
                    ? result
                    : null;
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

        // Get one event by ID.
        public async Task<JsonElement?> GetEventAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<JsonElement>(
                    $"api/events/{id}");
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

        // Create an event.
        public async Task<bool> CreateEventAsync(
            string token, object eventData)
        {
            LastError = null;

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "api/events");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(eventData);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                LastError = $"Status: {(int)response.StatusCode}. {error}";
                return false;
            }

            return true;
        }

        // Update an event.
        public async Task<bool> UpdateEventAsync(
            string token, int id, object eventData)
        {
            LastError = null;

            using var request = new HttpRequestMessage(
                HttpMethod.Put, $"api/events/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(eventData);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                LastError = $"Status: {(int)response.StatusCode}. {error}";
                return false;
            }

            return true;
        }

        // Delete an event.
        public async Task<bool> DeleteEventAsync(
            string token, int id)
        {
            LastError = null;

            using var request = new HttpRequestMessage(
                HttpMethod.Delete, $"api/events/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                LastError = $"Status: {(int)response.StatusCode}. {error}";
                return false;
            }

            return true;
        }
    }
}
