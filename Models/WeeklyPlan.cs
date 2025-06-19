using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run.Models
{
    public class WeeklyPlan
    {
        public string WeekTitle { get; set; }                
        public string Description { get; set; }              
        public List<DailyWorkout> Workouts { get; set; }      
        public string Focus { get; set; }                     
        public bool IsCompleted { get; set; }           
        public int WeekNumber { get; set; }                  
        public string Tips { get; set; }                  
        public string Intensity { get; set; }                 
    }
}

