using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using HabitGhost.Models;
using Dapper;

public static class UserAuthService
{
    private static readonly string connectionString = @"Server=VAG\SQLEXPRESS;Database=HabitGhostDB;Integrated Security=True;TrustServerCertificate=True;";

    public static User Authenticate(string username, string password)
    {
        string hashed = HashPassword(password);

        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = "SELECT Id, UserName, Mail, PasswordHash FROM Users WHERE UserName = @Username AND PasswordHash = @Hash";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Hash", hashed);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Mail = reader.GetString(2),
                            PasswordHash = reader.GetString(3)
                        };
                    }
                }
            }
        }

        return null;
    }

    public static string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public static void EnsureMachineRecord(int userId, string hwid)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string checkSql = "SELECT COUNT(*) FROM Machines WHERE UserId = @UserId AND Hwid = @Hwid";
            int exists = conn.ExecuteScalar<int>(checkSql, new { UserId = userId, Hwid = hwid });

            if (exists == 0)
            {
                string insertSql = @"
                INSERT INTO Machines (UserId, Hwid, OSVersion, LastOnline)
                VALUES (@UserId, @Hwid, @OS, @Now)";

                conn.Execute(insertSql, new
                {
                    UserId = userId,
                    Hwid = hwid,
                    OS = Environment.OSVersion.ToString(),
                    Now = DateTime.Now
                });
            }
            else
            {
                string updateSql = "UPDATE Machines SET LastOnline = @Now WHERE UserId = @UserId AND Hwid = @Hwid";
                conn.Execute(updateSql, new { UserId = userId, Hwid = hwid, Now = DateTime.Now });
            }
        }
    }

    public static string GenerateEmail(string username)
    {
        string emailUsername = username.Replace("_", ".").Replace(" ", ".").ToLower();
        return $"{emailUsername}@example.com";
    }

    public static User CreateUser(string username, string passwordHash, string email)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
                INSERT INTO Users (Username, PasswordHash, Mail)
                OUTPUT INSERTED.Id
                VALUES (@Username, @PasswordHash, @Mail)";

                int newId = conn.ExecuteScalar<int>(sql, new
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    Mail = email
                });

                return new User
                {
                    Id = newId,
                    Username = username,
                    PasswordHash = passwordHash,
                    Mail = email
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[!] Failed to create user: " + ex.Message);
            return null;
        }
    }

}
