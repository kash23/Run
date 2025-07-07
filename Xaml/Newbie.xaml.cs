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
                    newText = newText.Substring(0, 3); // Max 3 digits

                if (entry.Text != newText)
                    entry.Text = newText;
            }
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