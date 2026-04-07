using Guna.UI2.WinForms;
using HabitGhost.Additionals;
using HabitGhost.Models;
using HabitGhost.Notifications;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HabitGhost
{
    public partial class AddForm : Form
    {
        private MainForm _mainForm;
        private string _tag;


        public AddForm(string tag, MainForm mainForm)
        {
            InitializeComponent();
            ApplyTheme();
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
            _tag = tag;
            lblType.Text = $"Add a new {_tag}";

            _mainForm = mainForm;

            if (tag == "Habit")
            {
                guna2CmbBxFrequency.Visible = true;
                lblFreqDesc.Text = "Frequency:";
                guna2DteTmPkrReminder.Visible = true;
                lblReminderDue.Text = "Reminder time:";
            }
            else if (tag == "Goal")
            {
                guna2TxtBxDescription.Location = new System.Drawing.Point(guna2CmbBxFrequency.Location.X, guna2CmbBxFrequency.Location.Y + 6);
                guna2TxtBxDescription.Visible = true;
                lblFreqDesc.Text = "Description:";
                guna2DtTmPkrDueDate.Value = DateTime.Now.AddDays(1);
                guna2DtTmPkrDueDate.Location = guna2DteTmPkrReminder.Location;
                guna2DtTmPkrDueDate.Visible = true;
                lblReminderDue.Text = "Due date:";
            }
            else
            {
                NotificationManager.AddNotification("Invalid parameter", null);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void ThemeManager_ThemeChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }
        private void ApplyTheme()
        {
            ColorScheme theme = ThemeManager.CurrentTheme;

            this.BackColor = theme.Background;
            this.ForeColor = theme.Text;

            ApplyThemeToControls(this.Controls, theme);
        }
        private void ApplyThemeToControls(Control.ControlCollection controls, ColorScheme theme)
        {
            foreach (Control control in controls)
            {
                switch (control)
                {
                    case Guna2Button btn:
                        btn.FillColor = theme.Accent;
                        btn.ForeColor = theme.Text;
                        btn.HoverState.FillColor = ControlPaint.Light(theme.Accent);
                        break;

                    case Guna2TextBox txt:
                        txt.FillColor = theme.Secondary;
                        txt.ForeColor = theme.Text;
                        txt.HoverState.BorderColor = theme.Accent;
                        txt.BorderColor = theme.Accent;
                        txt.PlaceholderForeColor = ControlPaint.Light(theme.Text);
                        break;

                    case Guna2ComboBox combo:
                        combo.FillColor = theme.Secondary;
                        combo.ForeColor = theme.Text;
                        combo.BorderColor = theme.Accent;
                        break;

                    case Guna2Panel panel:
                        panel.FillColor = theme.Background;
                        break;

                    default:
                        control.BackColor = theme.Secondary;
                        control.ForeColor = theme.Text;
                        break;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, theme);
                }
            }
        }
        private void guna2BtnOk_Click(object sender, System.EventArgs e)
        {
            if (_tag == "Habit")
            {
                if (!string.IsNullOrWhiteSpace(guna2TxtBxTitle.Text))
                {
                    _mainForm.AddHabit(guna2TxtBxTitle.Text, guna2CmbBxFrequency.SelectedItem, guna2DteTmPkrReminder.Value);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationManager.AddNotification("Please fill in all fields", null);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(guna2TxtBxTitle.Text) && !string.IsNullOrWhiteSpace(guna2TxtBxDescription.Text) && guna2DtTmPkrDueDate.Value >= DateTime.Now)
                {
                    _mainForm.AddGoal(guna2TxtBxTitle.Text, guna2TxtBxDescription.Text, guna2DtTmPkrDueDate.Value);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationManager.AddNotification("Please fill in all fields", null);
            }
        }

        private void guna2BtnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None) this.DialogResult = DialogResult.Cancel;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // відписка від події, щоб уникнути memory leak
            ThemeManager.ThemeChanged -= ThemeManager_ThemeChanged;
            base.OnFormClosed(e);
        }
    }
}
