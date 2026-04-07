using Guna.UI2.WinForms;
using HabitGhost.Additionals;
using HabitGhost.Notifications;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HabitGhost
{
    public partial class NotificationForm : Form
    {
        private Timer _fade = new Timer { Interval = 10 };
        private Timer _closeTimer;
        private MainForm _mainForm;
        private bool _isMouseOver = false;
        private string _message;
        private Notification _notification;


        public NotificationForm(string message, MainForm baseForm)
        {
            InitializeComponent();
            SubscribeToChildMouseEvents(this);
            ThemeManager.ThemeChanged += (s, e) => UpdateTheme();
            UpdateTheme();
            _mainForm = baseForm;

            if (_mainForm == null)
            {
                guna2BtnSettings.Visible = false;
                guna2BtnRead.Visible = false;
            }

            var screenBounds = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screenBounds.Width - this.Width - 10, screenBounds.Height - this.Height - 10);

            _message = message;
            lblMessage.Text = _message;

            if (lblMessage.Width > 255 || message.Contains("\n"))
            {
                lblMessage.Text = NotificationManager.WrapText(message, 255, lblMessage.Font);

                int count = message.Count(c => c == '\n');
                if (count > 0) for (int i = 0; i < count; i++) this.Height += lblMessage.Height - 20;
                else this.Height += lblMessage.Height - 20;
            }

            _fade.Tick += (sender, e) =>
            {
                if (this.Opacity > 0) this.Opacity -= 0.05;
                else
                {
                    NotificationManager.RemoveNotification(this);
                    _fade.Stop();
                    this.Close();
                }
            };

            _notification = new Notification { Message = message, Timestamp = DateTime.Now };
            if (_mainForm != null) _mainForm.AddUnreadNotification(_message);
        }


        private void SubscribeToChildMouseEvents(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                child.MouseEnter += NotificationForm_MouseEnter;
                child.MouseLeave += NotificationForm_MouseLeave;
                if (child.HasChildren) SubscribeToChildMouseEvents(child);
            }
        }

        public void ShowNotification(int duration = 5000)
        {
            this.Show();

            _closeTimer = new Timer { Interval = duration };
            _closeTimer.Tick += (sender, e) =>
            {
                if (!_isMouseOver)
                {
                    _closeTimer.Stop();
                    _fade.Start();
                }
            };
            _closeTimer.Start();
        }


        private void guna2BtnSettings_Click(object sender, System.EventArgs e)
        {
            var tabControl = (Guna2TabControl)_mainForm.Controls["guna2TbCtrlMain"];
            tabControl.SelectedTab = tabControl.TabPages["tbPgSettings"];
        }

        private void guna2BtnClose_Click(object sender, System.EventArgs e)
        {
            NotificationManager.RemoveNotification(this);
            this.Close();
        }

        private void NotificationForm_MouseEnter(object sender, EventArgs e)
        {
            _isMouseOver = true;
            _closeTimer?.Stop();
            _fade.Stop();
            this.Opacity = 1;
        }

        private void NotificationForm_MouseLeave(object sender, EventArgs e)
        {
            _isMouseOver = false;

            var validationTimer = new Timer { Interval = 300 };
            validationTimer.Tick += (s, args) =>
            {
                validationTimer.Stop();
                if (!this.Bounds.Contains(Cursor.Position))
                {
                    _closeTimer?.Start();
                }
            };
            validationTimer.Start();
        }

        private void guna2BtnRead_Click(object sender, EventArgs e)
        {
            _mainForm.RemoveUnreadNotification(_notification.Message);
            NotificationManager.RemoveNotification(this);
            this.Close();
        }

        private void UpdateTheme()
        {
            // Основные цвета формы
            this.BackColor = ThemeManager.CurrentTheme.Background;
            this.ForeColor = ThemeManager.CurrentTheme.Text;

            // Обновление элементов управления
            lblTitle.ForeColor = ThemeManager.CurrentTheme.Text;
            lblMessage.ForeColor = ThemeManager.CurrentTheme.Text;
        }
    }
}
