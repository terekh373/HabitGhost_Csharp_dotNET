using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitGhost.Additionals
{
    public class GoalStatistics
    {
        public int TotalGoals { get; set; }
        public int CompletedGoals { get; set; }
        public int OverdueGoals { get; set; }
        public int PendingGoals => TotalGoals - CompletedGoals;
    }
}
