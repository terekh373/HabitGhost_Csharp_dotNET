namespace TcpServer.Models
{
    public class ClientRequest
    {
        public int UserId { get; set; } // ID користувача
        public string Token { get; set; } // PasswordHash (як токен)
        public string Hwid { get; set; } // Hardware ID машини
        public string RequestType { get; set; } // "data", "history", "goals"

        public List<Log> Logs { get; set; } = new();
        public List<HabitTracking> HabitTrackings { get; set; } = new();
        public new List<Habit> Habits { get; set; }
        public List<Goal> Goals { get; set; } = new();
    }

}
