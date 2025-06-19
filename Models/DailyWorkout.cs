using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run.Models
{
    public class DailyWorkout
    {
        public string Day { get; set; }           
        public string WorkoutType { get; set; }    
        public string Description { get; set; }    
        public bool IsCompleted { get; set; }      
    }
}
