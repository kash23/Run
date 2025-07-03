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
using Run.Services;


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
                if (IsSignUpMode)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter all required fields", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter registered mobile number and password.", "OK");
                }

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
                    //await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.Name}!", "OK");
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("UserPhone", MobileNumber);
                    await UserService.LoadUserAsync(MobileNumber);
                    Application.Current.MainPage = new NavigationPage(new Menu());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
                }
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
