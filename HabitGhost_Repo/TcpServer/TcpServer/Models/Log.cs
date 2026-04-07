namespace TcpServer.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string Content { get; set; }
        public DateTime LogTime { get; set; }
    }
}
