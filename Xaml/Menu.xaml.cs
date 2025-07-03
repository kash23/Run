using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Run.ViewModels;

namespace Run
{
    public partial class Menu : ContentPage
    {
        private NewbiePlanViewModel viewModel;
        public Menu()
        {
            InitializeComponent();
            //viewModel = new NewbiePlanViewModel();
            //BindingContext = viewModel;
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

