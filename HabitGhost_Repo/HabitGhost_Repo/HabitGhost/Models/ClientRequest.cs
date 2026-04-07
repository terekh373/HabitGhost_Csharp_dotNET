using HabitGhost.Models;
using System.Collections.Generic;

public class ClientRequest
{
    public int UserId { get; set; }
    public string Token { get; set; } // PasswordHash
    public string Hwid { get; set; }

    public string RequestType { get; set; } = "data"; // або "goals", "history"
    public new List<Goal> Goals { get; set; }
    public new List<Habit> Habits { get; set; }
    public new List<HabitTracking> HabitTrackings { get; set; }
    public new List<Log> Logs { get; set; }
}
