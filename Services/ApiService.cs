using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Run.Models;

namespace Run.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://fitnessak-geekfrhkb3f0fpay.canadacentral-01.azurewebsites.net/")
            };
        }

        public async Task<List<User>> GetUsersAsync(string bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/users");

            if (!string.IsNullOrEmpty(bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // Optionally log or throw exception
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<User> PostUserAsync(Workout newUser, string bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/users")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(newUser),
                    Encoding.UTF8,
                    "application/json")
            };

            if (!string.IsNullOrEmpty(bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request);

            // Read the response body (even if not successful)
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Log full details
                var errorMessage = $@"
                    Failed to create user.
                    Status: {response.StatusCode}
                    Reason: {response.ReasonPhrase}
                    Response body: {responseContent}";

                throw new Exception(errorMessage);
            }

            // Success: deserialize the response into a User
            return JsonSerializer.Deserialize<User>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<bool> UpdateUserAsync(int id, User user, string bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/users/{id}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json")
            };

            if (!string.IsNullOrEmpty(bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update user. Status: {response.StatusCode}, Error: {error}");
            }

            return true; // Update successful
        }

        // DELETE - Remove user
        public async Task<bool> DeleteUserAsync(int id, string bearerToken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/users/{id}");

            if (!string.IsNullOrEmpty(bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to delete user. Status: {response.StatusCode}, Error: {error}");
            }

            return true; // Delete successful
        }
    }
}
