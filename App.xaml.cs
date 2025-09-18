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

namespace Run
{
    public partial class App : Application
    { 
        //private readonly FirebaseService _firebaseService = new();
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Menu());
            InitializeApp();
        }

        private async void InitializeApp()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            string mobileNumber = Preferences.Get("UserPhone", string.Empty);

            if (isLoggedIn && !string.IsNullOrEmpty(mobileNumber))
            {
                await UserService.LoadUserAsync(mobileNumber);
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserPhone", mobileNumber);

                MainPage = new NavigationPage(new Menu());
            }

        }
    }
}
// AIzaSyAcelOj2e9qKfljoS1hCIrYIY8cEsfiDSA
// https://run2303app-default-rtdb.firebaseio.com/:null