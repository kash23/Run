using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Run.ViewModels;


namespace Run
{
    public partial class Newbie : ContentPage
    {
        private NewbiePlanViewModel viewModel;

        public Newbie()
        {
                InitializeComponent();
                viewModel = new NewbiePlanViewModel();
                BindingContext = viewModel;

        }

        private async void OnViewPlanClicked(object sender, EventArgs e)
        {
            await Task.Run(() => viewModel.LoadPlan());
        }
        
        private async void OnDownPlanClicked(object sender, EventArgs e)
        {
            await Task.Run(() => viewModel.ExportToPdf());
        }

    }
}