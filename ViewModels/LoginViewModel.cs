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


namespace Run.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _mobileNumber;
        private string _name;
        private int _age;
        private string _password;
        private int _height;
        private int _weight;
        public string MobileNumber
        {
            get => _mobileNumber;
            set { _mobileNumber = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public int Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }

        public int Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(nameof(Weight)); }
        }
        public string FormTitle => IsSignUpMode ? "Sign Up" : "Login";
        public string ToggleButtonText => IsSignUpMode ? "Switch to Login" : "Switch to Sign Up";
        public string SubmitButtonText => IsSignUpMode ? "Sign Up" : "Login";
        private bool _isSignUpMode = false;

        public bool IsSignUpMode
        {
            get => _isSignUpMode;
            set { _isSignUpMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(FormTitle)); OnPropertyChanged(nameof(ToggleButtonText)); OnPropertyChanged(nameof(SubmitButtonText)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand ToggleModeCommand { get; }
        public ICommand SubmitCommand { get; }
        private INavigation _navigation;
        private string sessionInfo = string.Empty;
        private readonly string firebaseApiKey = "AIzaSyAcelOj2e9qKfljoS1hCIrYIY8cEsfiDSA";
        private readonly FirebaseService _firebaseService = new();

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ToggleModeCommand = new Command(() => IsSignUpMode = !IsSignUpMode);
            SubmitCommand = new Command(async () => await OnSubmitAsync());

            SaveCommand = new Command(async () => await SaveUser());
           
        }
        private async Task OnSubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter mobile and password.", "OK");
                return;
            }

            if (IsSignUpMode)
            {
                var user = new UserData
                {
                    Id = MobileNumber,
                    Name = Name,
                    Age = Age,
                    Phone = MobileNumber,
                    Password = Password,
                    height = Height,
                    weight = Weight
                };

                await _firebaseService.SaveUserAsync(user);
                await Application.Current.MainPage.DisplayAlert("Success", "Account created!", "OK");
            }
            else
            {
                var user = await _firebaseService.GetUserAsync(MobileNumber);
                if (user != null && user.Password == Password)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.Name}!", "OK");
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("UserPhone", MobileNumber);
                    Application.Current.MainPage = new NavigationPage(new Menu());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
                }
            }
        }
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
        private async Task SaveUser()
        {
            var user = new UserData
            {
                Id = MobileNumber,
                Name = Name,
                Age = Age,
                Phone = MobileNumber,
                Password = Password,
                height = Height,
                weight = Weight
            };

            await _firebaseService.SaveUserAsync(user);

            var retrieved = await _firebaseService.GetUserAsync(MobileNumber);
            if (retrieved != null)
                await Application.Current.MainPage.DisplayAlert("Success", $"Name: {retrieved.Name}", "OK");
        }

/*        private async Task SendOtp()
        {
            using var client = new HttpClient();

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:sendVerificationCode?key={firebaseApiKey}";

            var requestBody = new
            {
                phoneNumber = $"+91{MobileNumber}",
                recaptchaToken = "ignored" // recaptchaToken is ignored if SafetyNet is disabled in Firebase settings
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<SendOtpResponse>(responseText);
                sessionInfo = result.sessionInfo;
                IsOtpEnabled = true;
                await Application.Current.MainPage.DisplayAlert("OTP Sent", "OTP has been sent to your number.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", responseText, "OK");
            }
        }
        private async Task VerifyOtp()
        {
            using var client = new HttpClient();

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPhoneNumber?key={firebaseApiKey}";

            var requestBody = new
            {
                sessionInfo = sessionInfo,
                code = Otp
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<VerifyOtpResponse>(responseText);
                await Application.Current.MainPage.DisplayAlert("Success", "OTP verified successfully!", "OK");
                // Navigate to next page here
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Verification Failed", responseText, "OK");
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
