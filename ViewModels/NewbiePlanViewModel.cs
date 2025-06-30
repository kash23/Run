using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Collections.Generic;
using Run.Models;

namespace Run.ViewModels
{
    public class NewbiePlanViewModel : INotifyPropertyChanged
    {
        private string _goal;
        private string _activityLevel;
        private string _daysPerWeek;
        private string _runStyle;
        private string _planSummary;
        private string weight;
        private string height;
        private List<string> generatedPlanLines = new();
        public ObservableCollection<WeeklyPlan> WeeklyPlans { get; set; } = new();

        public ObservableCollection<string> Goals { get; set; }
        public ObservableCollection<string> ActivityLevels { get; set; }
        public ObservableCollection<string> DaysPerWeekOptions { get; set; }
        public ObservableCollection<string> RunStyles { get; set; }

        public string Goal
        {
            get => _goal;
            set => SetProperty(ref _goal, value);
        }

        public string ActivityLevel
        {
            get => _activityLevel;
            set => SetProperty(ref _activityLevel, value);
        }

        public string DaysPerWeek
        {
            get => _daysPerWeek;
            set => SetProperty(ref _daysPerWeek, value);
        }

        public string RunStyle
        {
            get => _runStyle;
            set => SetProperty(ref _runStyle, value);
        }

        public string Weight
        {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        public string Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }

        public string PlanSummary
        {
            get => _planSummary;
            set => SetProperty(ref _planSummary, value);
        }

        public ICommand LoadPlanCommand { get; }
        public ICommand ExportToPdfCommand { get; }

        public NewbiePlanViewModel()
        {
            Goals = new ObservableCollection<string> { "Weight Loss", "Endurance", "Speed", "General Fitness" };
            ActivityLevels = new ObservableCollection<string> { "Sedentary", "Lightly Active", "Moderately Active", "Very Active" };
            DaysPerWeekOptions = new ObservableCollection<string> { "3 days", "4 days", "5 days" };
            RunStyles = new ObservableCollection<string> { "Long Runs", "Interval Training", "Hill Workouts" };

            LoadPlanCommand = new Command(LoadPlan);
            ExportToPdfCommand = new Command(ExportToPdf);

/*            WeeklyPlans = new ObservableCollection<WeeklyPlan>();
            ToggleExpandCommand = new Command<WeeklyPlan>((plan) =>
            {
                if (plan != null)
                    plan.IsExpanded = !plan.IsExpanded;
            });*/
        }

        public void LoadPlan()
        {
            string bmiNote = string.Empty;
            double bmi = 0;

            if (double.TryParse(Weight, out var w) && double.TryParse(Height, out var h))
            {
                var heightM = h / 100;
                bmi = w / (heightM * heightM);

                string bmiCategory = bmi switch
                {
                    < 18.5 => "Underweight",
                    >= 18.5 and < 25 => "Normal weight",
                    >= 25 and < 30 => "Overweight",
                    _ => "Obese"
                };

                bmiNote = $"📊 BMI: {Math.Round(bmi, 1)} ({bmiCategory})";
            }
            else
            {
                bmiNote = $"📊 BMI: Not available (invalid height/weight)";
            }

            int days = DaysPerWeek.StartsWith("3") ? 3 :
                       DaysPerWeek.StartsWith("4") ? 4 :
                       DaysPerWeek.StartsWith("5") ? 5 : 3;

            WeeklyPlans.Clear();

            for (int week = 1; week <= 4; week++)
            {
                var weekPlan = new WeeklyPlan
                {
                    WeekNumber = week,
                    WeekTitle = $"WEEK {week}",
                    Focus = Goal,
                    Intensity = Goal == "Speed" ? "High" : Goal == "Endurance" ? "Medium" : "Low",
                    Tips = Goal switch
                    {
                        "Weight Loss" => "Stay consistent and hydrate!",
                        "Endurance" => "Focus on pacing and breathing.",
                        "Speed" => "Prioritize recovery between efforts.",
                        _ => "Listen to your body and rest if needed."
                    },
                    Workouts = new List<DailyWorkout>()
                };

                for (int day = 1; day <= days; day++)
                {
                    string desc = Goal switch
                    {
                        "Weight Loss" => (week, day) switch
                        {
                            (_, 1) => $"Run/walk - Run {1 + week} min / Walk 2 min x 4",
                            (_, 2) => "Bodyweight strength or yoga",
                            (_, 3) => "Long walk + core stretch",
                            (_, 4) => "Cross-training or easy jog",
                            (_, 5) => $"Long slow jog {30 + week * 5} min",
                            _ => ""
                        },
                        "Endurance" => (week, day) switch
                        {
                            (_, 1) => $"Steady run - {30 + week * 5} min",
                            (_, 2) => "Recovery walk or swim",
                            (_, 3) => $"Tempo run {15 + week * 2} min",
                            (_, 4) => "Core + mobility",
                            (_, 5) => $"Long run {50 + week * 5} min",
                            _ => ""
                        },
                        "Speed" => (week, day) switch
                        {
                            (_, 1) => $"Hill sprints x {4 + week} + Jog",
                            (_, 2) => "Easy jog + form drills",
                            (_, 3) => $"Intervals 400m x {3 + week}",
                            (_, 4) => "Strength + flexibility",
                            (_, 5) => "Short time trial (1-3K)",
                            _ => ""
                        },
                        _ => (week, day) switch
                        {
                            (_, 1) => $"Easy run {20 + week * 2} min",
                            (_, 2) => "Walk + stretching",
                            (_, 3) => "Run/walk combo",
                            (_, 4) => "Cross training or yoga",
                            (_, 5) => "Long jog + recovery",
                            _ => ""
                        }
                    };

                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        weekPlan.Workouts.Add(new DailyWorkout
                        {
                            Day = $"Day {day}",
                            WorkoutType = Goal,
                            Description = desc,
                            IsCompleted = false
                        });
                    }
                }

                WeeklyPlans.Add(weekPlan);
            }

            PlanSummary = $"🌟 Goal: {Goal}\n" +
                          $"⚡ Activity Level: {ActivityLevel}\n" +
                          $"🗓️ Days per Week: {DaysPerWeek}\n" +
                          $"🏃‍♂️ Run Style: {RunStyle}\n" +
                          $"📊 Weight: {Weight} kg | Height: {Height} cm\n" +
                          $"{bmiNote}\n";
        }

        public async void ExportToPdf()
        {
            if (WeeklyPlans == null || WeeklyPlans.Count == 0)
                return;

            var document = new PdfDocument();
            var frontPage = document.AddPage();
            var gfx1 = XGraphics.FromPdfPage(frontPage);
            var fontTitle = new XFont("Verdana", 18, XFontStyle.Bold);
            var fontBody = new XFont("Verdana", 12, XFontStyle.Regular);

            gfx1.DrawString("\ud83c\udfc3 Personalized Running Plan", fontTitle, XBrushes.DarkBlue,
                new XRect(0, 40, frontPage.Width, 40), XStringFormats.TopCenter);

            int top = 100;
            int spacing = 30;
            gfx1.DrawString($"\ud83c\udf1f Goal: {Goal}", fontBody, XBrushes.Black, 40, top); top += spacing;
            gfx1.DrawString($"\u26a1 Activity Level: {ActivityLevel}", fontBody, XBrushes.Black, 40, top); top += spacing;
            gfx1.DrawString($"\ud83d\uddd5\ufe0f Days per Week: {DaysPerWeek}", fontBody, XBrushes.Black, 40, top); top += spacing;
            gfx1.DrawString($"\ud83c\udfc3\u200d\u2642\ufe0f Run Style: {RunStyle}", fontBody, XBrushes.Black, 40, top); top += spacing;
            gfx1.DrawString($"\ud83d\udcca Weight: {Weight} kg | Height: {Height} cm", fontBody, XBrushes.Black, 40, top); top += spacing;
            gfx1.DrawString(generatedPlanLines.FirstOrDefault(l => l.StartsWith("\ud83d\udcca")) ?? "", fontBody, XBrushes.Black, 40, top);

            var planPage = document.AddPage();
            var gfx2 = XGraphics.FromPdfPage(planPage);
            gfx2.DrawString("\ud83d\udccb 4-Week Plan", fontTitle, XBrushes.DarkGreen,
                new XRect(0, 40, planPage.Width, 40), XStringFormats.TopCenter);

            int top2 = 100;
            foreach (var line in generatedPlanLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    top2 += spacing / 2;
                    continue;
                }

                gfx2.DrawString(line, fontBody, XBrushes.Black, 40, top2);
                top2 += spacing;
            }

            var fileName = $"NewbiePlan_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            using (var stream = File.Create(filePath))
                document.Save(stream);

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
