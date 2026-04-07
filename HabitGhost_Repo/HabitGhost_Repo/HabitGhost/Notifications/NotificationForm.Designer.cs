namespace HabitGhost
{
    partial class NotificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2BrdrlsFormNotification = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.guna2BtnRead = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnLogo = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnSettings = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnClose = new Guna.UI2.WinForms.Guna2Button();
            this.tlTpMain = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // guna2BrdrlsFormNotification
            // 
            this.guna2BrdrlsFormNotification.BorderRadius = 8;
            this.guna2BrdrlsFormNotification.ContainerControl = this;
            this.guna2BrdrlsFormNotification.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BrdrlsFormNotification.DragForm = false;
            this.guna2BrdrlsFormNotification.TransparentWhileDrag = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.ForeColor = System.Drawing.Color.LightGray;
            this.lblTitle.Location = new System.Drawing.Point(39, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(87, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Habit manager";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Bahnschrift Condensed", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMessage.Location = new System.Drawing.Point(13, 42);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(71, 21);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "<message>";
            // 
            // guna2BtnRead
            // 
            this.guna2BtnRead.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.BackgroundImage = global::HabitGhost.Properties.Resources.read;
            this.guna2BtnRead.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnRead.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnRead.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnRead.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnRead.Location = new System.Drawing.Point(236, 13);
            this.guna2BtnRead.Name = "guna2BtnRead";
            this.guna2BtnRead.Size = new System.Drawing.Size(16, 16);
            this.guna2BtnRead.TabIndex = 11;
            this.tlTpMain.SetToolTip(this.guna2BtnRead, "Mark as read");
            this.guna2BtnRead.Click += new System.EventHandler(this.guna2BtnRead_Click);
            // 
            // guna2BtnLogo
            // 
            this.guna2BtnLogo.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnLogo.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnLogo.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.DisabledState.Image = global::HabitGhost.Properties.Resources.logo;
            this.guna2BtnLogo.Enabled = false;
            this.guna2BtnLogo.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnLogo.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnLogo.Image = global::HabitGhost.Properties.Resources.logo;
            this.guna2BtnLogo.ImageOffset = new System.Drawing.Point(1, 0);
            this.guna2BtnLogo.ImageSize = new System.Drawing.Size(16, 16);
            this.guna2BtnLogo.Location = new System.Drawing.Point(11, 10);
            this.guna2BtnLogo.Name = "guna2BtnLogo";
            this.guna2BtnLogo.Size = new System.Drawing.Size(20, 20);
            this.guna2BtnLogo.TabIndex = 10;
            // 
            // guna2BtnSettings
            // 
            this.guna2BtnSettings.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.BackgroundImage = global::HabitGhost.Properties.Resources.settings;
            this.guna2BtnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnSettings.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnSettings.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnSettings.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnSettings.Location = new System.Drawing.Point(200, 13);
            this.guna2BtnSettings.Name = "guna2BtnSettings";
            this.guna2BtnSettings.Size = new System.Drawing.Size(16, 16);
            this.guna2BtnSettings.TabIndex = 9;
            this.guna2BtnSettings.Click += new System.EventHandler(this.guna2BtnSettings_Click);
            // 
            // guna2BtnClose
            // 
            this.guna2BtnClose.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.BackgroundImage = global::HabitGhost.Properties.Resources.close;
            this.guna2BtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnClose.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnClose.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnClose.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnClose.Location = new System.Drawing.Point(272, 13);
            this.guna2BtnClose.Name = "guna2BtnClose";
            this.guna2BtnClose.Size = new System.Drawing.Size(16, 16);
            this.guna2BtnClose.TabIndex = 7;
            this.guna2BtnClose.Click += new System.EventHandler(this.guna2BtnClose_Click);
            // 
            // tlTpMain
            // 
            this.tlTpMain.AutoPopDelay = 2000;
            this.tlTpMain.BackColor = System.Drawing.Color.Transparent;
            this.tlTpMain.ForeColor = System.Drawing.Color.Transparent;
            this.tlTpMain.InitialDelay = 500;
            this.tlTpMain.ReshowDelay = 100;
            this.tlTpMain.ShowAlways = true;
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(300, 76);
            this.Controls.Add(this.guna2BtnRead);
            this.Controls.Add(this.guna2BtnLogo);
            this.Controls.Add(this.guna2BtnSettings);
            this.Controls.Add(this.guna2BtnClose);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTitle);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "NotificationForm";
            this.TopMost = true;
            this.MouseEnter += new System.EventHandler(this.NotificationForm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.NotificationForm_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BrdrlsFormNotification;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Button guna2BtnClose;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSettings;
        private Guna.UI2.WinForms.Guna2Button guna2BtnLogo;
        private Guna.UI2.WinForms.Guna2Button guna2BtnRead;
        private System.Windows.Forms.ToolTip tlTpMain;
    }
}