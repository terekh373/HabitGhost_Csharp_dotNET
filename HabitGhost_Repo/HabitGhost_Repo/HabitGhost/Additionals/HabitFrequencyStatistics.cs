using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitGhost.Additionals
{
    public class HabitFrequencyStatistics
    {
        public string HabitName { get; set; }
        public string Frequency { get; set; }
        public int CompletedCount { get; set; }
        public int ExpectedCount { get; set; }
        public double CompletionRate => ExpectedCount == 0 ? 0 : (double)CompletedCount / ExpectedCount * 100;
    }
}
