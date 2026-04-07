using System;
using System.Collections.Generic;

namespace HabitGhost.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Frequency { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public User User { get; set; }
        public ICollection<HabitTracking> HabitTrackings { get; set; }
    }
}
