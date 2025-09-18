using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Run.Services;
using Run.ViewModels; 

namespace Run
{
    public partial class Menu : ContentPage
    {

        public Menu()
        {
            InitializeComponent();
        }
        private async void OnGoToPageAClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Newbie());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnGoToPageBClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnGoToPageCClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Workouts());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

}

