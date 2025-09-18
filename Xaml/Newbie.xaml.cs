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
        private void OnNumericEntryChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                string newText = new string(e.NewTextValue
                    .Where(char.IsDigit) 
                    .ToArray());

                if (newText.Length > 3)
                    newText = newText.Substring(0, 3);  

                if (entry.Text != newText)
                    entry.Text = newText;
            }
        }
        private async void OnViewPlanClicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.Goal != null &&
                viewModel.ActivityLevel != null &&
                viewModel.DaysPerWeek != null &&
                viewModel.RunStyle != null &&
                viewModel.Weight != null &&
                viewModel.Height != null)
                {
                    /*var viewModel = (NewbiePlanViewModel)this.BindingContext;
                    viewModel.LoadPlanCommand.Execute(null);*/
                    downloadnewbie.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("Missing Information", "Please fill in all required fields before proceeding.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnViewPlanClicked: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while loading the plan. Please try again.", "OK");
            }
        }
        private async void OnDownPlanClicked(object sender, EventArgs e)
        {
            await Task.Run(() => viewModel.ExportToPdf());
        }
    }
}