using Microsoft.Maui.Controls;
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
                System.Diagnostics.Debug.WriteLine($"❌ Navigation failed: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnGoToPageBClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage()); 
        }
    }
}

