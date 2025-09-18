using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Run.Models;
using Run.Services;
using System.Windows.Input;
 


namespace Run.ViewModels
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        private User user = new User();
        public ObservableCollection<Workout> Workouts { get; set; } = new ObservableCollection<Workout>();
        private readonly ApiService _apiService = new ApiService();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand DeleteWorkoutCommand { get; }
        public ICommand EditWorkoutCommand { get; }
        public ICommand CreateWorkoutCommand { get; }
        public UsersViewModel()
        {
            DeleteWorkoutCommand = new Command<Workout>(OnDeleteWorkout);
            EditWorkoutCommand = new Command<Workout>(OnEditWorkout);
            CreateWorkoutCommand = new Command(OnCreateWorkout);
        }
        private void OnDeleteWorkout(Workout workout)
        {
            if (workout != null && Workouts.Contains(workout))
            {
                Workouts.Remove(workout); 
            }
        }
        private async void OnEditWorkout(Workout workout)
        {
            if (workout == null) return;

            // For now, just display an alert
            await Application.Current.MainPage.DisplayAlert(
                "Edit Workout",
                $"Editing {workout.Name} ({workout.DurationMinutes} min)",
                "OK"
            );

            // Later: Navigate to an EditPage and pass the workout for update
        }
        private async void OnCreateWorkout()
        {
            Workout wrk = new Workout();

            wrk.Name = "Push Ups";
            wrk.Date = DateTime.Now;
            wrk.DurationMinutes = 30;
            wrk.UserId = user.Id;
            //wrk.Id = 2;
            user.Workouts.Clear();
            user.Workouts.Add( wrk );



            var Workouts = await _apiService.PostUserAsync(wrk, null);


            // Later: Navigate to a WorkoutCreatePage
            // await Shell.Current.GoToAsync(nameof(WorkoutCreatePage));
        }
        public async Task LoadUsers()
        {
            
            var users = await _apiService.GetUsersAsync("your_token_here");

            user = users.FirstOrDefault();

            if (users != null)
            {
                Workouts.Clear();
                foreach (var user in users)
                {
                    foreach (var workout in user.Workouts)
                    {
                        Workouts.Add(workout);
                    }
                }
                    
            }

            OnCreateWorkout();
        }

        
    }
}
