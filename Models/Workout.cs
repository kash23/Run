namespace Run.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public bool IsSelected { get; set; }
        // public User User { get; set; }
    }
}
