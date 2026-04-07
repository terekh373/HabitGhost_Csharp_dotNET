using System;
using System.Collections.Generic;

namespace HabitGhost.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Hwid { get; set; }
        public string OsVersion { get; set; }
        public DateTime? LastOnline { get; set; }
        public User User { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}
