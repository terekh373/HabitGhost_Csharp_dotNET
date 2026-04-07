using HabitGhost.Additionals;
using HabitGhost.Notifications;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HabitGhost
{
    public partial class NotificationItem : UserControl
    {
        private Notification _notification;
        private MainForm _mainForm;


        public NotificationItem(Notification notification, MainForm mainForm)
        {
            InitializeComponent();
            ApplyTheme();
            ThemeManager.ThemeChanged += OnThemeChanged;
            _notification = notification;
            _mainForm = mainForm;

            lblMessage.Text = notification.Message;
            if (lblMessage.Width > 400 || notification.Message.Contains("\n"))
            {
                lblMessage.Text = NotificationManager.WrapText(notification.Message, 400, lblMessage.Font);

                int count = notification.Message.ToString().Count(c => c == '\n');
                if (count > 0) for (int i = 0; i < count; i++) this.Height += lblMessage.Height - 20;
                else this.Height += lblMessage.Height - 20;
            }

            guna2BtnDel.Location = new Point(520, (this.Size.Height - guna2BtnDel.Size.Height) / 2);

            lblTime.Location = new Point(this.Size.Width - lblTime.Size.Width - 2, this.Size.Height - lblTime.Size.Height - 2);
            lblTime.Text = notification.Timestamp.ToString("HH:mm");

            this.MouseEnter += NotificationItem_MouseEnter;
            this.MouseLeave += NotificationItem_MouseLeave;
            foreach (Control item in this.Controls)
            {
                item.MouseEnter += NotificationItem_MouseEnter;
                item.MouseLeave += NotificationItem_MouseLeave;
            }
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }
        private void ApplyTheme()
        {
            var theme = ThemeManager.CurrentTheme;

            this.BackColor = theme.Background;
            lblMessage.ForeColor = theme.Text;
            lblTime.ForeColor = theme.Text;
        }
        private void guna2BtnDel_Click(object sender, System.EventArgs e) => _mainForm.RemoveUnreadNotification(_notification.Message);

        public void Cleanup()
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
        }

        private void NotificationItem_MouseEnter(object sender, EventArgs e)
        {
            var theme = ThemeManager.CurrentTheme;
            this.BackColor = theme.Secondary;
            foreach (Control item in this.Controls) item.BackColor = theme.Secondary;
        }
        private void NotificationItem_MouseLeave(object sender, EventArgs e)
        {
            var theme = ThemeManager.CurrentTheme;
            this.BackColor = theme.Background;
            foreach (Control item in this.Controls) item.BackColor = theme.Background;
        }
    }
}
