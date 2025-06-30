using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PdfSharpCore.Drawing;
using QuestPDF.Fluent;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Microsoft.Maui.Storage;
using System.IO;
using Run.Models;
using Run.Models;
using Run.Models;

namespace Run.ViewModels
{
    public class TrainingPlanViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Distances { get; set; } = new() { "5K", "10K", "Half Marathon", "Full Marathon" };
        public ObservableCollection<string> Levels { get; set; } = new() { "Beginner", "Intermediate", "Advanced" };

       // public  ICommand ExportToPdfCommand => new Command(() => ExportPlanToPdf());

       public ICommand ExportToPdfCommand => new Command(ExportPlanToPdf);

        private string selectedDistance;
        public string SelectedDistance
        {
            get => selectedDistance;
            set
            {
                if (selectedDistance != value)
                {
                    selectedDistance = value;
                    OnPropertyChanged();
                    LoadPlan();
                }
            }
        }

        private string selectedLevel;
        public string SelectedLevel
        {
            get => selectedLevel;
            set
            {
                if (selectedLevel != value)
                {
                    selectedLevel = value;
                    OnPropertyChanged();
                    LoadPlan();
                }
            }
        }

        private TrainingPlan selectedPlan;
        public TrainingPlan SelectedPlan
        {
            get => selectedPlan;
            set
            {
                selectedPlan = value;
                OnPropertyChanged();
            }
        }
        public ICommand ToggleExpandCommand { get; }
        public TrainingPlanViewModel()
        {
            SelectedPlan = new TrainingPlan();
            SelectedPlan.Weeks = new ObservableCollection<WeeklyPlan>();

            ToggleExpandCommand = new Command<WeeklyPlan>((plan) =>
            {
                if (plan != null)
                    plan.IsExpanded = !plan.IsExpanded;
            });
        }
        private void LoadPlan()
        {
            if (string.IsNullOrEmpty(SelectedDistance) || string.IsNullOrEmpty(SelectedLevel))
                return;

            ObservableCollection<WeeklyPlan> weeks = new();
            string focus = string.Empty;

            int totalWeeks = SelectedDistance switch
            {
                "5K" => SelectedLevel switch
                {
                    "Beginner" => 8,
                    "Intermediate" => 6,
                    "Advanced" => 8,
                    _ => 6
                },
                "10K" => SelectedLevel switch
                {
                    "Beginner" => 8,
                    "Intermediate" => 8,
                    "Advanced" => 10,
                    _ => 8
                },
                "Half Marathon" => SelectedLevel switch
                {
                    "Beginner" => 12,
                    "Intermediate" => 12,
                    "Advanced" => 14,
                    _ => 12
                },
                "Full Marathon" => SelectedLevel switch
                {
                    "Beginner" => 20,
                    "Intermediate" => 16,
                    "Advanced" => 20,
                    _ => 16
                },
                _ => 0
            };

            for (int i = 1; i <= totalWeeks; i++)
            {
                int baseKm = SelectedDistance switch
                {
                    "5K" => 3,
                    "10K" => 5,
                    "Half Marathon" => 8,
                    "Full Marathon" => 12,
                    _ => 4
                };

                int progression = (int)(baseKm + (i * 0.75)); // progress slowly week by week
                int longRunKm = baseKm + (i * 1); // long run increases more aggressively
                int tempoKm = (int)(baseKm + (i * 0.5));
                int recoveryKm = Math.Max(2, baseKm / 2);

                weeks.Add(new WeeklyPlan
                {
                    WeekTitle = $"Week {i}",
                    WeekNumber = i,
                    Description = $"{SelectedLevel} {SelectedDistance} Plan – Week {i}",
                    Focus = i % 4 == 0 ? "Recovery & Adaptation" : "Progression",
                    Intensity = i % 4 == 0 ? "Easy" : "Moderate",
                    Tips = i % 4 == 0 ? "Deload week, stay light" : "Push your limits gradually",
                    IsCompleted = false,
                    Workouts = new List<DailyWorkout>
                    {
                        new DailyWorkout { Day = "Monday :", WorkoutType = " Rest", Description = " Rest day", IsCompleted = false },
                        new DailyWorkout { Day = "Tuesday :", WorkoutType = " Run", Description = $" Run {progression} km", IsCompleted = false },
                        new DailyWorkout { Day = "Wednesday :", WorkoutType = " Cross Training", Description = " Optional bike/swim", IsCompleted = false },
                        new DailyWorkout { Day = "Thursday :", WorkoutType = " Tempo Run", Description = $" Tempo run {tempoKm} km", IsCompleted = false },
                        new DailyWorkout { Day = "Friday :", WorkoutType = " Rest", Description = " Rest or walk", IsCompleted = false },
                        new DailyWorkout { Day = "Saturday :", WorkoutType = " Long Run", Description = $" Long run {longRunKm} km", IsCompleted = false },
                        new DailyWorkout { Day = "Sunday :", WorkoutType = " Recovery Run", Description = $" Recovery run {recoveryKm} km", IsCompleted = false }
                    }
                });
            }

            focus = SelectedDistance switch
            {
                "5K" => SelectedLevel switch
                {
                    "Beginner" => "Build basic running habit",
                    "Intermediate" => "Improve pace and form",
                    "Advanced" => "Sub-20 min 5K training",
                    _ => "Improve 5K fitness"
                },
                "10K" => SelectedLevel switch
                {
                    "Beginner" => "Build stamina and distance",
                    "Intermediate" => "Build endurance and speed",
                    "Advanced" => "Race-specific peak training",
                    _ => "10K performance"
                },
                "Half Marathon" => SelectedLevel switch
                {
                    "Beginner" => "Build to 21K finish",
                    "Intermediate" => "Build race pace endurance",
                    "Advanced" => "Sub-2 hour half marathon",
                    _ => "Half marathon improvement"
                },
                "Full Marathon" => SelectedLevel switch
                {
                    "Beginner" => "Finish the marathon safely",
                    "Intermediate" => "Endurance and pace mix",
                    "Advanced" => "Sub-4 or Boston Qualifier",
                    _ => "Marathon prep"
                },
                _ => "Running training"
            };


            SelectedPlan = new TrainingPlan
            {
                Distance = SelectedDistance,
                Level = SelectedLevel,
                DurationWeeks = weeks.Count,
                Focus = focus,
                Weeks = weeks
            };
        }
        private string GetDayWithSuffix(int day)
        {
            if (day % 100 >= 11 && day % 100 <= 13)
                return $"{day}th";

            return (day % 10) switch
            {
                1 => $"{day}st",
                2 => $"{day}nd",
                3 => $"{day}rd",
                _ => $"{day}th"
            };
        }
        public async void ExportPlanToPdf()
        {
            if (SelectedPlan == null) return;

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new MauiFontResolver();
            var fontTitle = new XFont("OpenSans", 16, XFontStyle.Bold);
            var fontBody = new XFont("OpenSans", 12, XFontStyle.Regular);

            double y = 40;
       
            DateTime now = DateTime.Now;

            string dayWithSuffix = GetDayWithSuffix(now.Day);

            gfx.DrawString($"Training Plan - {SelectedPlan.Distance} ({SelectedPlan.Level})", fontTitle, XBrushes.Black, new XPoint(40, y));
            y += 30;

            foreach (var week in SelectedPlan.Weeks)
            {
                gfx.DrawString($"{week.WeekTitle}: {week.Description}", fontBody, XBrushes.Black, new XPoint(40, y));
                y += 20;

                foreach (var workout in week.Workouts)
                {
                    gfx.DrawString($"• {workout.Day} {workout.WorkoutType} - {workout.Description}", fontBody, XBrushes.Gray, new XPoint(60, y));
                    y += 18;
                }

                y += 10;

                if (y > page.Height - 60)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }

            // Save
            string path = Path.Combine(FileSystem.CacheDirectory, "TrainingPlan.pdf");
            using var stream = File.Create(path);
            document.Save(stream);
            await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
