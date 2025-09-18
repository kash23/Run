using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.OS;
using Android.Views;
using Android.OS;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Platform;


//using Firebase;

namespace Run
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar; // light icons off
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#000000"));
        }
    }
}
