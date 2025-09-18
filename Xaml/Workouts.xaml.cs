using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Run.ViewModels;


namespace Run
{
    public partial class Workouts : ContentPage
    {
        private UsersViewModel viewModel;
        public Workouts()
        {
            InitializeComponent();
            viewModel = new UsersViewModel();
            BindingContext = viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Load users from API
            await viewModel.LoadUsers();
        }
    }
}
