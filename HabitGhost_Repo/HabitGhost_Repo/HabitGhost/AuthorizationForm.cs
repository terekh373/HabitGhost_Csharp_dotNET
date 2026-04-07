using Guna.UI2.WinForms;
using HabitGhost.Additionals;
using HabitGhost.Models;
using HabitGhost.Notifications;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HabitGhost
{
    public partial class AuthorizationForm : Form
    {
        private bool _hidePassword = true;
        private TcpClientManager _tcpClient;

        public AuthorizationForm(TcpClientManager tcpClient)
        {
            InitializeComponent();
            _tcpClient = tcpClient;


            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
            this.Disposed += (s, e) => ThemeManager.ThemeChanged -= ThemeManager_ThemeChanged;

            UpdateTheme();
        }

        private void ThemeManager_ThemeChanged(object sender, EventArgs e)
        {
            UpdateTheme();
        }
        private void UpdateTheme()
        {
            this.SuspendLayout();

            // Основные цвета
            this.BackColor = ThemeManager.CurrentTheme.Background;
            this.ForeColor = ThemeManager.CurrentTheme.Text;

            // Обновление элементов
            UpdateControlColors(this.Controls);

            // Специальные элементы
            UpdateSpecialElements();

            this.ResumeLayout(true);
        }
        private void UpdateControlColors(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (!(control is Guna2Button))
                {
                    // Общие свойства
                    control.BackColor = ThemeManager.CurrentTheme.Background;
                    control.ForeColor = ThemeManager.CurrentTheme.Text;

                    // Обработка Guna2 элементов
                    if (control is Guna2TextBox textBox)
                    {
                        textBox.FillColor = ThemeManager.CurrentTheme.Secondary;
                        textBox.BorderColor = ThemeManager.CurrentTheme.Accent;
                    }
                    else if (control is Guna2CustomCheckBox checkBox)
                    {
                        checkBox.CheckedState.FillColor = ThemeManager.CurrentTheme.Accent;
                    }

                    // Рекурсивный обход
                    if (control.HasChildren)
                    {
                        UpdateControlColors(control.Controls);
                    }
                }
            }
        }
        private void UpdateSpecialElements()
        {
            // Обновление цвета вкладок
            guna2TbCntrlAuth.TabMenuBackColor = ThemeManager.CurrentTheme.Background;
            guna2TbCntrlAuth.TabButtonIdleState.FillColor = ThemeManager.CurrentTheme.Secondary;
            guna2TbCntrlAuth.TabButtonSelectedState.FillColor = ThemeManager.CurrentTheme.Accent;
        }

        private void guna2TxtBxNamePassword_TextChanged(object sender, EventArgs e)
        {
            var txtbx = (Guna2TextBox)sender;
            if (txtbx.Tag.ToString() == "Uname")
            {
                if (guna2TxtBxName.Text.Length > 32)
                {
                    guna2TxtBxName.Text = guna2TxtBxName.Text.Substring(0, 32);
                    guna2TxtBxName.SelectionStart = 32;

                    NotificationManager.AddNotification("Max username length is 32 characters", null);
                }
            }
            else
            {
                if (guna2TxtBxPassword.Text.Length > 21)
                {
                    guna2TxtBxPassword.Text = guna2TxtBxPassword.Text.Substring(0, 21);
                    guna2TxtBxPassword.SelectionStart = 21;

                    NotificationManager.AddNotification("Max password length is 21 character", null);
                }
            }

            if (!String.IsNullOrWhiteSpace(guna2TxtBxName.Text) && guna2TxtBxName.Text.Length >= 2 && guna2TxtBxName.Text.Length <= 32 && Regex.IsMatch(guna2TxtBxPassword.Text, @"^(?=.*\d)(?=.*[A-Z])(?=.*[!@#$%^&*()_+]).{8,21}$"))
            {
                guna2BtnNext.Visible = true;
            }
            else
            {
                guna2BtnNext.Visible = false;
            }
        }

        private void guna2TxtBxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isBackspace = e.KeyChar == (char)Keys.Back;
            e.Handled = !isBackspace && guna2TxtBxName.Text.Length >= 32 ? true : false;
        }
        private void guna2TxtBxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isBackspace = e.KeyChar == (char)Keys.Back;
            e.Handled = !isBackspace && guna2TxtBxPassword.Text.Length >= 21 ? true : false;
        }

        private async void guna2BtnNext_Click(object sender, EventArgs e)
        {
            string username = guna2TxtBxName.Text.Trim();
            string password = guna2TxtBxPassword.Text.Trim();

            string hwid = MachineGuid.GetMachineGuid();
            string passwordHash = UserAuthService.HashPassword(password);

            // 1. Спроба входу
            User user = UserAuthService.Authenticate(username, password);

            // 2. Якщо користувача немає — йдемо далі налаштовувати
            if (user == null) guna2TbCntrlAuth.SelectedTab = tbPgSecond;
            else
            {
                // 3. Додаємо запис у Machines (якщо ще немає)
                UserAuthService.EnsureMachineRecord(user.Id, hwid);

                // 4. Авторизаційний запит на сервер
                var authRequest = new ClientRequest
                {
                    UserId = user.Id,
                    Token = passwordHash,
                    Hwid = hwid,
                    RequestType = "auth"
                };

                string authJson = JsonConvert.SerializeObject(authRequest, Formatting.Indented);
                string response = await _tcpClient.SendJsonAsync(authJson);

                if (!response.Contains("OK"))
                {
                    NotificationManager.AddNotification("Server did not authentificate the user.", null);
                    return;
                }

                var userSet = UserSettings.Load();

                this.Hide();
                MainForm mainForm = new MainForm(userSet.Username, userSet.RememberMe, userSet.NotificationsEnabled, userSet.AutostartEnabled, _tcpClient);
                mainForm.FormClosed += (s, args) => this.Close();
                mainForm.Show();
            }
        }


        private async void guna2BtnSetUp_Click(object sender, EventArgs e)
        {
            string username = guna2TxtBxName.Text.Trim();
            string password = guna2TxtBxPassword.Text.Trim();

            string hwid = MachineGuid.GetMachineGuid();
            string passwordHash = UserAuthService.HashPassword(password);

            // 1. Спроба входу
            string email = UserAuthService.GenerateEmail(username);
            User user = UserAuthService.CreateUser(username, passwordHash, email);

            if (user == null)
            {
                NotificationManager.AddNotification("The user was not created.", null);
                return;
            }

            NotificationManager.AddNotification("The user was created.", null);

            // 3. Додаємо запис у Machines (якщо ще немає)
            UserAuthService.EnsureMachineRecord(user.Id, hwid);

            // 4. Авторизаційний запит на сервер
            var authRequest = new ClientRequest
            {
                UserId = user.Id,
                Token = passwordHash,
                Hwid = hwid,
                RequestType = "auth"
            };

            string authJson = JsonConvert.SerializeObject(authRequest, Formatting.Indented);
            string response = await _tcpClient.SendJsonAsync(authJson);

            if (!response.Contains("OK"))
            {
                NotificationManager.AddNotification("Server did not authentificate the user.", null);
                return;
            }

            // 5. Зберігаємо локально
            UserSettings settings = new UserSettings
            {
                Username = user.Username,
                PasswordHash = passwordHash,
                RememberMe = guna2CtmCkBxRemember.Checked,
                NotificationsEnabled = guna2CtmCkBxNotifications.Checked,
                AutostartEnabled = guna2CtmCkBxAutostart.Checked
            };

            UserSettings.Save(settings);

            this.Hide();
            MainForm mainForm = new MainForm(user.Username, settings.RememberMe, settings.NotificationsEnabled, settings.AutostartEnabled, _tcpClient);
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }


        private void guna2BtnBack_Click(object sender, EventArgs e) => guna2TbCntrlAuth.SelectedTab = tbPgFirst;

        private void guna2BtnShowHide_Click(object sender, EventArgs e)
        {
            _hidePassword = !_hidePassword;
            guna2TxtBxPassword.UseSystemPasswordChar = _hidePassword;
            guna2BtnShowHide.BackgroundImage = _hidePassword ? Properties.Resources.hide : Properties.Resources.show;
            tlTpMain.SetToolTip(guna2BtnShowHide, _hidePassword ? "Show password" : "Hide password");
        }
    }
}
