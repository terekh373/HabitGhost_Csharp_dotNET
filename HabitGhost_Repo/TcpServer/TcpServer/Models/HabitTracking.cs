namespace TcpServer.Models
{
    public class HabitTracking
    {
        public int TrackingId { get; set; }
        public int HabitId { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
