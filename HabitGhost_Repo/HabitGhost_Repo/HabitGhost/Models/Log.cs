using System;

namespace HabitGhost.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string Content { get; set; }
        public DateTime LogTime { get; set; }
        public Machine Machine { get; set; }
    }
}
