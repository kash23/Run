using System.ComponentModel.DataAnnotations;

namespace Run.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}
