using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Run.Services;
using Run.ViewModels;

namespace Run.Controls
{
    public partial class TopBar : ContentView
    {
        public TopBar()
        {
            InitializeComponent();
            if (UserService.CurrentUser != null)
                UserName = $"{UserService.CurrentUser.Name} 👋";
                //UserName = $"Hi, {UserService.CurrentUser.Name} 👋";
        }
        public static readonly BindableProperty PageTitleProperty =
        BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(TopBar), string.Empty);
        private void OnLogoutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsLoggedIn", false);
            Preferences.Remove("UserPhone");
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private void OnProfileClicked(object sender, EventArgs e)
        {
            // Navigate to profile page or show popup
        }
        public string PageTitle
        { 
            get => (string)GetValue(PageTitleProperty);
            set => SetValue(PageTitleProperty, value);
        }
        public static readonly BindableProperty UserNameProperty =
    BindableProperty.Create(nameof(UserName), typeof(string), typeof(TopBar), string.Empty);

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        public static readonly BindableProperty ShowBackButtonProperty =
            BindableProperty.Create(nameof(ShowBackButton), typeof(bool), typeof(TopBar), false);

        public bool ShowBackButton
        {
            get => (bool)GetValue(ShowBackButtonProperty);
            set => SetValue(ShowBackButtonProperty, value);
        }

        public static readonly BindableProperty ProfileCommandProperty =
            BindableProperty.Create(nameof(ProfileCommand), typeof(ICommand), typeof(TopBar));

        public ICommand ProfileCommand
        {
            get => (ICommand)GetValue(ProfileCommandProperty);
            set => SetValue(ProfileCommandProperty, value);
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(".."); // or Navigation.PopAsync() if not using Shell
        }
    }
}

