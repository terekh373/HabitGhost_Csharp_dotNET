using Newtonsoft.Json;
using System;
using System.IO;

namespace HabitGhost.Additionals
{
    public class UserSettings
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool RememberMe { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool AutostartEnabled { get; set; }
        public bool DarkTheme { get; set; } = false;

        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userSettings.json");

        public static void Save(UserSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static UserSettings Load()
        {
            if (!File.Exists(FilePath)) return null;

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<UserSettings>(json);
        }

        public static void Clear()
        {
            var settings = Load();
            settings.RememberMe = false;
            Save(settings);
        }
    }
}
