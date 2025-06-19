using System.Collections.ObjectModel;

namespace Run.Models
{
    public class TrainingPlan
    {
        public string Distance { get; set; }
        public string Level { get; set; }
        public int DurationWeeks { get; set; }
        public string Focus { get; set; }
        public ObservableCollection<WeeklyPlan> Weeks { get; set; }
    }
}
