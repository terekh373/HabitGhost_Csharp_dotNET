using HabitGhost.Additionals;
using HabitGhost.Models;
using HabitGhost.Notifications;
using System;
using System.Windows.Forms;

namespace HabitGhost
{
    public partial class HideAllNotificationsForm : Form
    {
        private Timer fade = new Timer { Interval = 10 };


        public HideAllNotificationsForm()
        {
            InitializeComponent();

            this.Click += (s, e) => NotificationManager.HideAllNotifications();
            label1.Click += (s, e) => NotificationManager.HideAllNotifications();

            fade.Tick += (sender, e) =>
            {
                if (this.Opacity > 0) this.Opacity -= 0.05;
                else CloseForm();
            };
            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var theme = ThemeManager.CurrentTheme;

            this.BackColor = theme.Background;
            this.ForeColor = theme.Text;

            label1.ForeColor = theme.Text;
        }

        public void ShowForm()
        {
            this.Show();
        }

        public void CloseForm()
        {
            NotificationManager.ResetHideAllNotifications();
            fade.Stop();
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }
    }
}
