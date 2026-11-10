using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class EnrolmentApiService
    {
        private readonly HttpClient _httpClient;

        public EnrolmentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateEnrolmentAsync(
            string token,
            int categoryId,
            int raceNumber)
        {
            var enrolment = new
            {
                categoryID = categoryId,
                raceNumber = raceNumber
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "api/enrolments");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(enrolment);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<JsonElement?> GetMyEnrolmentsAsync(string token)
        {
            try
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Get, "api/enrolments/my");

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