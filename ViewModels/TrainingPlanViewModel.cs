using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Microsoft.Maui.Storage;
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
                weeks.Add(new WeeklyPlan
                {
                    WeekTitle = $"Week {i}",
                    WeekNumber = i,
                    Description = $"Sample plan for {SelectedLevel} {SelectedDistance}, week {i}",
                    Focus = i % 4 == 0 ? "Recovery & Adaptation" : "Progression",
                    Intensity = i % 4 == 0 ? "Easy" : "Moderate",
                    Tips = i % 4 == 0 ? "Deload week, stay light" : "Focus on improving distance or speed",
                    IsCompleted = false,
                    Workouts = new List<DailyWorkout>
            {
                new DailyWorkout { Day = "Monday", WorkoutType = "Rest", Description = "Recovery day", IsCompleted = false },
                new DailyWorkout { Day = "Tuesday", WorkoutType = "Run", Description = "Easy run", IsCompleted = false },
                new DailyWorkout { Day = "Wednesday", WorkoutType = "Cross Training", Description = "Bike/Swim or Strength", IsCompleted = false },
                new DailyWorkout { Day = "Thursday", WorkoutType = "Tempo Run", Description = "Run at steady effort", IsCompleted = false },
                new DailyWorkout { Day = "Friday", WorkoutType = "Rest", Description = "Optional light yoga", IsCompleted = false },
                new DailyWorkout { Day = "Saturday", WorkoutType = "Long Run", Description = $"Run progressively longer: Week {i} km base", IsCompleted = false },
                new DailyWorkout { Day = "Sunday", WorkoutType = "Walk or Easy Run", Description = "Light recovery movement", IsCompleted = false },
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

        public async void ExportPlanToPdf()
        {
            TrainingPlan plan = SelectedPlan;
            using var document = new PdfDocument();
            var page = document.Pages.Add();

            var font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
            page.Graphics.DrawString($"Training Plan - {SelectedPlan.Distance} ({SelectedPlan.Level})", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

            float y = 25;
            foreach (var week in SelectedPlan.Weeks)
            {
                page.Graphics.DrawString($"{week.WeekTitle}: {week.Description}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
                y += 20;
            }

            string path = Path.Combine(FileSystem.CacheDirectory, "Plan.pdf");
            using var fileStream = File.Create(path);
            document.Save(fileStream);
            fileStream.Close();

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Training Plan",
                File = new ShareFile(path)
            });



        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
