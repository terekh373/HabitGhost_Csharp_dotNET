using System;
using System.Collections.Generic;
using System.Linq;

namespace HabitGhost.Models
{
    public class HabitStatistics
    {
        public string HabitName { get; set; }
        public List<DateTime> CompletedDates { get; set; } = new List<DateTime>();

        public int GetCompletedCountLastDays(int days)
        {
            DateTime fromDate = DateTime.Now.Date.AddDays(-days + 1);
            return CompletedDates.Count(d => d.Date >= fromDate);
        }

        public double GetCompletionRate(int expectedDays)
        {
            int done = GetCompletedCountLastDays(expectedDays);
            return expectedDays == 0 ? 0 : (double)done / expectedDays * 100;
        }

        public Dictionary<string, int> GetPerDayCount(int days = 7)
        {
            var stats = new Dictionary<string, int>();
            DateTime today = DateTime.Today;
            for (int i = 0; i < days; i++)
            {
                var day = today.AddDays(-i);
                string label = day.ToString("ddd"); // Mon, Tue, Wed
                int count = CompletedDates.Count(d => d.Date == day.Date);
                stats[label] = count;
            }
            return stats.Reverse().ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
