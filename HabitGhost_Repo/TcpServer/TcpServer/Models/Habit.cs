namespace TcpServer.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Frequency { get; set; }
        public TimeSpan? ReminderTime { get; set; }
    }
}
