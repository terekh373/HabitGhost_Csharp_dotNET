using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace HabitGhost.Additionals
{
    public class AppSettings
    {
        public string Username { get; set; }
        public bool IsNotificationsEnabled { get; set; }
        public bool IsAutostartEnabled { get; set; }
        public bool IsRemember { get; set; }

        private static readonly string path = Path.Combine(Application.StartupPath, "settings.json");

        public static void Save(AppSettings settings)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        public static AppSettings Load()
        {
            if (File.Exists(path))
                return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
            return new AppSettings();
        }
    }
}
