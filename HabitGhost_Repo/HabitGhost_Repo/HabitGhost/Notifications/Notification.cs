using System;

namespace HabitGhost.Notifications
{
    public class Notification
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
