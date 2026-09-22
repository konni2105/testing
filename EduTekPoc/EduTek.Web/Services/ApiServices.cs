using EduTek.Web.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EduTek.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await ExecuteWithRefreshAsync(
                client => client.GetAsync(endpoint));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // POST
        public async Task<string> PostAsync(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);

            var response = await ExecuteWithRefreshAsync(
                client => client.PostAsync(
                    endpoint,
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json")));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // PUT
        public async Task<string> PutAsync(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);

            var response = await ExecuteWithRefreshAsync(
                client => client.PutAsync(
                    endpoint,
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json")));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // DELETE
        public async Task<string> DeleteAsync(string endpoint)
        {
            var response = await ExecuteWithRefreshAsync(
                client => client.DeleteAsync(endpoint));

            var responseContent =
                await response.Content.ReadAsStringAsync();

            return $"{(int)response.StatusCode} - {responseContent}";
        }

        // Refresh Token API
        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var client =
                _httpClientFactory.CreateClient("EduTekAPI");

            var data = new
            {
                RefreshToken = refreshToken
            };

            var json = JsonSerializer.Serialize(data);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "api/Auth/refresh-token",
                content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // Common method for GET / POST / PUT / DELETE
        private async Task<HttpResponseMessage> ExecuteWithRefreshAsync(
    Func<HttpClient, Task<HttpResponseMessage>> action)
        {
            var client =
                _httpClientFactory.CreateClient("EduTekAPI");

            // 1. Get current Access Token
            var accessToken =
                _httpContextAccessor.HttpContext?
                    .Session.GetString("AccessToken");

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        accessToken);
            }

            // 2. First API request
            var response = await action(client);

            // 3. Only refresh when API returns 401
            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                var refreshToken =
                    _httpContextAccessor.HttpContext?
                        .Session.GetString("RefreshToken");

                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new UnauthorizedAccessException(
                        "Refresh token not found in session.");
                }

                // 4. Request new tokens from API
                var refreshResponse =
                    await RefreshTokenAsync(refreshToken);

                var authResponse =
                    JsonSerializer.Deserialize<AuthResponseModel>(
                        refreshResponse,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (authResponse == null ||
                    string.IsNullOrEmpty(authResponse.Token))
                {
                    throw new UnauthorizedAccessException(
                        "Unable to refresh access token.");
                }

                // 5. Store new tokens in MVC Session
                _httpContextAccessor.HttpContext?
                    .Session.SetString(
                        "AccessToken",
                        authResponse.Token);

                _httpContextAccessor.HttpContext?
                    .Session.SetString(
                        "RefreshToken",
                        authResponse.RefreshToken);

                // 6. Attach new Access Token
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        authResponse.Token);

                // 7. Retry original request
                response = await action(client);
            }

            return response;
        }
    }
}