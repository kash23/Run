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
        private readonly FirebaseService _firebaseService = new();
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            string MobileNumber = Preferences.Get("UserPhone", string.Empty);
            if (isLoggedIn)
            {
                var user =  _firebaseService.GetUserAsync(MobileNumber);
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserPhone", MobileNumber);
                UserService.LoadUserAsync(MobileNumber);

                MainPage = new NavigationPage(new Menu());
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
            //   101988493212
        }
    }
}
// AIzaSyAcelOj2e9qKfljoS1hCIrYIY8cEsfiDSA
// https://run2303app-default-rtdb.firebaseio.com/:null