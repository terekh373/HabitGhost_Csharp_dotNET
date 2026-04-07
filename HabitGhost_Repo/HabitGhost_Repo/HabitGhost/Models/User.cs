using System.Collections.Generic;

namespace HabitGhost.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Machine> Machines { get; set; }
        public ICollection<Habit> Habits { get; set; }
        public ICollection<Goal> Goals { get; set; }
    }
}
