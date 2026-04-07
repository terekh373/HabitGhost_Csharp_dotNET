namespace HabitGhost
{
    partial class NotificationItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.guna2BtnDel = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblMessage.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblMessage.Location = new System.Drawing.Point(26, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(64, 19);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "<message>";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTime.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTime.Location = new System.Drawing.Point(556, 37);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(40, 19);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "<time>";
            // 
            // guna2BtnDel
            // 
            this.guna2BtnDel.Animated = true;
            this.guna2BtnDel.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.BackgroundImage = global::HabitGhost.Properties.Resources.close;
            this.guna2BtnDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnDel.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnDel.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnDel.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnDel.Location = new System.Drawing.Point(520, 19);
            this.guna2BtnDel.Name = "guna2BtnDel";
            this.guna2BtnDel.Size = new System.Drawing.Size(20, 20);
            this.guna2BtnDel.TabIndex = 9;
            this.guna2BtnDel.Click += new System.EventHandler(this.guna2BtnDel_Click);
            // 
            // NotificationItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.guna2BtnDel);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblMessage);
            this.Name = "NotificationItem";
            this.Size = new System.Drawing.Size(598, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTime;
        private Guna.UI2.WinForms.Guna2Button guna2BtnDel;
    }
}
