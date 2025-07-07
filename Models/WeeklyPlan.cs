using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public bool IsExpanded { get; set; } = false;
    }
}

