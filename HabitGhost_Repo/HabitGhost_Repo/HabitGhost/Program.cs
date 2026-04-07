using HabitGhost.Additionals;
using HabitGhost.Notifications;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HabitGhost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
        [STAThread]
        static void Main()
        {
            ThemeManager.CurrentTheme = ColorScheme.Light;
            NotificationManager.SetNotificationsEnabled(AppSettings.Load().IsNotificationsEnabled);
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "HabitGhostMutex", out createdNew))
            {
                if (!createdNew)
                {
                    // Уже запущен - выводим существующее окно
                    var current = Process.GetCurrentProcess();
                    var existing = Process.GetProcessesByName(current.ProcessName)
                        .FirstOrDefault(p => p.Id != current.Id);

                    if (existing != null)
                    {
                        ShowWindow(existing.MainWindowHandle, SW_RESTORE);
                        SetForegroundWindow(existing.MainWindowHandle);
                    }
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                TcpClientManager tcpClientManager = new TcpClientManager();
                tcpClientManager.ConnectAsync();

                var settings = UserSettings.Load();

                if (settings != null && settings.RememberMe)
                {
                    MainForm mainForm = new MainForm(
                        settings.Username,
                        settings.RememberMe,
                        settings.NotificationsEnabled,
                        settings.AutostartEnabled,
                        tcpClientManager
                    );

                    mainForm.FormClosed += (s, args) => Application.Exit();
                    Application.Run(mainForm);
                }
                else
                {
                    Application.Run(new AuthorizationForm(tcpClientManager));
                }
            }
        }
    }
}
