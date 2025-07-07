using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Collections.Generic;
using Run.Models;
using System.Text.Json;
using System.Text;

namespace Run.Services
{
    public class FirebaseService
    {
        private readonly HttpClient _httpClient;
        private const string FirebaseUrl = "https://run2303app-default-rtdb.firebaseio.com/";

        public FirebaseService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SaveUserAsync(UserData user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{FirebaseUrl}users/{user.Id}.json", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<UserData?> GetUserAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{FirebaseUrl}users/{userId}.json");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserData>(json);
        }
    }
}
