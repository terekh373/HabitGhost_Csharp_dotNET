namespace HabitGhost
{
    partial class AuthorizationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizationForm));
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.guna2TxtBxName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblWelcomeAddition = new System.Windows.Forms.Label();
            this.lblUnameHint = new System.Windows.Forms.Label();
            this.guna2TbCntrlAuth = new Guna.UI2.WinForms.Guna2TabControl();
            this.tbPgFirst = new System.Windows.Forms.TabPage();
            this.guna2BtnShowHide = new Guna.UI2.WinForms.Guna2Button();
            this.lblPassHint = new System.Windows.Forms.Label();
            this.lblRemember = new System.Windows.Forms.Label();
            this.guna2CtmCkBxRemember = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.guna2TxtBxPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2BtnNext = new Guna.UI2.WinForms.Guna2Button();
            this.lblCountPageOne = new System.Windows.Forms.Label();
            this.tbPgSecond = new System.Windows.Forms.TabPage();
            this.guna2BtnBack = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnSetUp = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CtmCkBxAutostart = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.guna2CtmCkBxNotifications = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblTitleTwo = new System.Windows.Forms.Label();
            this.lblAutostart = new System.Windows.Forms.Label();
            this.lblNotifications = new System.Windows.Forms.Label();
            this.lblCountPageTwo = new System.Windows.Forms.Label();
            this.tlTpMain = new System.Windows.Forms.ToolTip(this.components);
            this.guna2TbCntrlAuth.SuspendLayout();
            this.tbPgFirst.SuspendLayout();
            this.tbPgSecond.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWelcome.Location = new System.Drawing.Point(8, 8);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(302, 78);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome to Habit Manager!\r\nLet`s get started";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsername.Location = new System.Drawing.Point(154, 181);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(238, 23);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "How would You like to be addressed:";
            // 
            // guna2TxtBxName
            // 
            this.guna2TxtBxName.Animated = true;
            this.guna2TxtBxName.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TxtBxName.BorderRadius = 6;
            this.guna2TxtBxName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TxtBxName.DefaultText = "";
            this.guna2TxtBxName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TxtBxName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TxtBxName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TxtBxName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TxtBxName.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2TxtBxName.FocusedState.BorderColor = System.Drawing.Color.Black;
            this.guna2TxtBxName.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2TxtBxName.ForeColor = System.Drawing.Color.Black;
            this.guna2TxtBxName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2TxtBxName.Location = new System.Drawing.Point(398, 168);
            this.guna2TxtBxName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.guna2TxtBxName.Name = "guna2TxtBxName";
            this.guna2TxtBxName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.guna2TxtBxName.PlaceholderText = "Your name";
            this.guna2TxtBxName.SelectedText = "";
            this.guna2TxtBxName.Size = new System.Drawing.Size(238, 48);
            this.guna2TxtBxName.TabIndex = 2;
            this.guna2TxtBxName.Tag = "Uname";
            this.guna2TxtBxName.TextChanged += new System.EventHandler(this.guna2TxtBxNamePassword_TextChanged);
            this.guna2TxtBxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.guna2TxtBxName_KeyPress);
            // 
            // lblWelcomeAddition
            // 
            this.lblWelcomeAddition.Font = new System.Drawing.Font("Bahnschrift Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWelcomeAddition.ForeColor = System.Drawing.Color.Silver;
            this.lblWelcomeAddition.Location = new System.Drawing.Point(331, 31);
            this.lblWelcomeAddition.Name = "lblWelcomeAddition";
            this.lblWelcomeAddition.Size = new System.Drawing.Size(383, 48);
            this.lblWelcomeAddition.TabIndex = 3;
            this.lblWelcomeAddition.Text = "Build better habits and take full control of your routine. Our app helps you stay" +
    " consistent, track progress, and create lasting change — simple, powerful, and m" +
    "ade for you.";
            this.lblWelcomeAddition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnameHint
            // 
            this.lblUnameHint.AutoSize = true;
            this.lblUnameHint.Font = new System.Drawing.Font("Bahnschrift Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUnameHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblUnameHint.Location = new System.Drawing.Point(418, 147);
            this.lblUnameHint.Name = "lblUnameHint";
            this.lblUnameHint.Size = new System.Drawing.Size(201, 16);
            this.lblUnameHint.TabIndex = 5;
            this.lblUnameHint.Text = "Must be under 32 and more than 2 characters";
            // 
            // guna2TbCntrlAuth
            // 
            this.guna2TbCntrlAuth.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.guna2TbCntrlAuth.Controls.Add(this.tbPgFirst);
            this.guna2TbCntrlAuth.Controls.Add(this.tbPgSecond);
            this.guna2TbCntrlAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2TbCntrlAuth.ItemSize = new System.Drawing.Size(180, 40);
            this.guna2TbCntrlAuth.Location = new System.Drawing.Point(0, 0);
            this.guna2TbCntrlAuth.Name = "guna2TbCntrlAuth";
            this.guna2TbCntrlAuth.SelectedIndex = 0;
            this.guna2TbCntrlAuth.Size = new System.Drawing.Size(800, 450);
            this.guna2TbCntrlAuth.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TbCntrlAuth.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TbCntrlAuth.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TbCntrlAuth.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.guna2TbCntrlAuth.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TbCntrlAuth.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TbCntrlAuth.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TbCntrlAuth.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TbCntrlAuth.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.guna2TbCntrlAuth.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TbCntrlAuth.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TbCntrlAuth.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.guna2TbCntrlAuth.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TbCntrlAuth.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.guna2TbCntrlAuth.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2TbCntrlAuth.TabButtonSize = new System.Drawing.Size(180, 40);
            this.guna2TbCntrlAuth.TabIndex = 6;
            this.guna2TbCntrlAuth.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TbCntrlAuth.TabMenuVisible = false;
            // 
            // tbPgFirst
            // 
            this.tbPgFirst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.tbPgFirst.Controls.Add(this.guna2BtnShowHide);
            this.tbPgFirst.Controls.Add(this.lblPassHint);
            this.tbPgFirst.Controls.Add(this.lblRemember);
            this.tbPgFirst.Controls.Add(this.guna2CtmCkBxRemember);
            this.tbPgFirst.Controls.Add(this.lblPassword);
            this.tbPgFirst.Controls.Add(this.guna2TxtBxPassword);
            this.tbPgFirst.Controls.Add(this.guna2BtnNext);
            this.tbPgFirst.Controls.Add(this.lblCountPageOne);
            this.tbPgFirst.Controls.Add(this.lblWelcome);
            this.tbPgFirst.Controls.Add(this.lblUnameHint);
            this.tbPgFirst.Controls.Add(this.lblUsername);
            this.tbPgFirst.Controls.Add(this.guna2TxtBxName);
            this.tbPgFirst.Controls.Add(this.lblWelcomeAddition);
            this.tbPgFirst.Location = new System.Drawing.Point(5, 4);
            this.tbPgFirst.Name = "tbPgFirst";
            this.tbPgFirst.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgFirst.Size = new System.Drawing.Size(791, 442);
            this.tbPgFirst.TabIndex = 0;
            // 
            // guna2BtnShowHide
            // 
            this.guna2BtnShowHide.Animated = true;
            this.guna2BtnShowHide.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.BackgroundImage = global::HabitGhost.Properties.Resources.hide;
            this.guna2BtnShowHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnShowHide.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnShowHide.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnShowHide.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnShowHide.Location = new System.Drawing.Point(655, 268);
            this.guna2BtnShowHide.Name = "guna2BtnShowHide";
            this.guna2BtnShowHide.Size = new System.Drawing.Size(24, 24);
            this.guna2BtnShowHide.TabIndex = 16;
            this.tlTpMain.SetToolTip(this.guna2BtnShowHide, "Show password");
            this.guna2BtnShowHide.Click += new System.EventHandler(this.guna2BtnShowHide_Click);
            // 
            // lblPassHint
            // 
            this.lblPassHint.AutoSize = true;
            this.lblPassHint.Font = new System.Drawing.Font("Bahnschrift Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPassHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblPassHint.Location = new System.Drawing.Point(410, 221);
            this.lblPassHint.Name = "lblPassHint";
            this.lblPassHint.Size = new System.Drawing.Size(215, 32);
            this.lblPassHint.TabIndex = 15;
            this.lblPassHint.Text = "Must be between 8 and 21 characters, contain\r\nan uppercase letter, a digit and a " +
    "special symbol";
            this.lblPassHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRemember
            // 
            this.lblRemember.AutoSize = true;
            this.lblRemember.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRemember.Location = new System.Drawing.Point(288, 322);
            this.lblRemember.Name = "lblRemember";
            this.lblRemember.Size = new System.Drawing.Size(104, 23);
            this.lblRemember.TabIndex = 14;
            this.lblRemember.Text = "Remember me:";
            // 
            // guna2CtmCkBxRemember
            // 
            this.guna2CtmCkBxRemember.Animated = true;
            this.guna2CtmCkBxRemember.Checked = true;
            this.guna2CtmCkBxRemember.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxRemember.CheckedState.BorderRadius = 2;
            this.guna2CtmCkBxRemember.CheckedState.BorderThickness = 0;
            this.guna2CtmCkBxRemember.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxRemember.Location = new System.Drawing.Point(398, 325);
            this.guna2CtmCkBxRemember.Name = "guna2CtmCkBxRemember";
            this.guna2CtmCkBxRemember.Size = new System.Drawing.Size(20, 20);
            this.guna2CtmCkBxRemember.TabIndex = 13;
            this.guna2CtmCkBxRemember.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CtmCkBxRemember.UncheckedState.BorderRadius = 2;
            this.guna2CtmCkBxRemember.UncheckedState.BorderThickness = 0;
            this.guna2CtmCkBxRemember.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPassword.Location = new System.Drawing.Point(256, 268);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(136, 23);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Enter the password:";
            // 
            // guna2TxtBxPassword
            // 
            this.guna2TxtBxPassword.Animated = true;
            this.guna2TxtBxPassword.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TxtBxPassword.BorderRadius = 6;
            this.guna2TxtBxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TxtBxPassword.DefaultText = "";
            this.guna2TxtBxPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TxtBxPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TxtBxPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TxtBxPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TxtBxPassword.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2TxtBxPassword.FocusedState.BorderColor = System.Drawing.Color.Black;
            this.guna2TxtBxPassword.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2TxtBxPassword.ForeColor = System.Drawing.Color.Black;
            this.guna2TxtBxPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2TxtBxPassword.Location = new System.Drawing.Point(398, 255);
            this.guna2TxtBxPassword.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.guna2TxtBxPassword.Name = "guna2TxtBxPassword";
            this.guna2TxtBxPassword.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.guna2TxtBxPassword.PlaceholderText = "Password";
            this.guna2TxtBxPassword.SelectedText = "";
            this.guna2TxtBxPassword.Size = new System.Drawing.Size(238, 48);
            this.guna2TxtBxPassword.TabIndex = 11;
            this.guna2TxtBxPassword.Tag = "Pass";
            this.guna2TxtBxPassword.UseSystemPasswordChar = true;
            this.guna2TxtBxPassword.TextChanged += new System.EventHandler(this.guna2TxtBxNamePassword_TextChanged);
            this.guna2TxtBxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.guna2TxtBxPassword_KeyPress);
            // 
            // guna2BtnNext
            // 
            this.guna2BtnNext.Animated = true;
            this.guna2BtnNext.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.BackgroundImage = global::HabitGhost.Properties.Resources.next;
            this.guna2BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnNext.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnNext.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnNext.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnNext.Location = new System.Drawing.Point(361, 361);
            this.guna2BtnNext.Name = "guna2BtnNext";
            this.guna2BtnNext.Size = new System.Drawing.Size(72, 36);
            this.guna2BtnNext.TabIndex = 9;
            this.guna2BtnNext.Visible = false;
            this.guna2BtnNext.Click += new System.EventHandler(this.guna2BtnNext_Click);
            // 
            // lblCountPageOne
            // 
            this.lblCountPageOne.AutoSize = true;
            this.lblCountPageOne.Font = new System.Drawing.Font("Bahnschrift Condensed", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCountPageOne.ForeColor = System.Drawing.Color.DimGray;
            this.lblCountPageOne.Location = new System.Drawing.Point(755, 0);
            this.lblCountPageOne.Name = "lblCountPageOne";
            this.lblCountPageOne.Size = new System.Drawing.Size(40, 33);
            this.lblCountPageOne.TabIndex = 6;
            this.lblCountPageOne.Text = "1/2";
            // 
            // tbPgSecond
            // 
            this.tbPgSecond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.tbPgSecond.Controls.Add(this.guna2BtnBack);
            this.tbPgSecond.Controls.Add(this.guna2BtnSetUp);
            this.tbPgSecond.Controls.Add(this.guna2CtmCkBxAutostart);
            this.tbPgSecond.Controls.Add(this.guna2CtmCkBxNotifications);
            this.tbPgSecond.Controls.Add(this.lblTitleTwo);
            this.tbPgSecond.Controls.Add(this.lblAutostart);
            this.tbPgSecond.Controls.Add(this.lblNotifications);
            this.tbPgSecond.Controls.Add(this.lblCountPageTwo);
            this.tbPgSecond.Location = new System.Drawing.Point(5, 4);
            this.tbPgSecond.Name = "tbPgSecond";
            this.tbPgSecond.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgSecond.Size = new System.Drawing.Size(791, 442);
            this.tbPgSecond.TabIndex = 1;
            // 
            // guna2BtnBack
            // 
            this.guna2BtnBack.Animated = true;
            this.guna2BtnBack.BackColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.BackgroundImage = global::HabitGhost.Properties.Resources.back;
            this.guna2BtnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.guna2BtnBack.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.Cursor = System.Windows.Forms.Cursors.Default;
            this.guna2BtnBack.DisabledState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.DisabledState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.Font = new System.Drawing.Font("Impact", 21.75F);
            this.guna2BtnBack.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.HoverState.BorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.HoverState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.HoverState.ForeColor = System.Drawing.Color.Transparent;
            this.guna2BtnBack.Location = new System.Drawing.Point(355, 341);
            this.guna2BtnBack.Name = "guna2BtnBack";
            this.guna2BtnBack.Size = new System.Drawing.Size(72, 36);
            this.guna2BtnBack.TabIndex = 15;
            this.guna2BtnBack.Click += new System.EventHandler(this.guna2BtnBack_Click);
            // 
            // guna2BtnSetUp
            // 
            this.guna2BtnSetUp.Animated = true;
            this.guna2BtnSetUp.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2BtnSetUp.BorderRadius = 6;
            this.guna2BtnSetUp.BorderThickness = 2;
            this.guna2BtnSetUp.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSetUp.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSetUp.DisabledState.FillColor = System.Drawing.Color.Transparent;
            this.guna2BtnSetUp.DisabledState.ForeColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSetUp.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2BtnSetUp.Font = new System.Drawing.Font("Bahnschrift Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnSetUp.ForeColor = System.Drawing.Color.Black;
            this.guna2BtnSetUp.Location = new System.Drawing.Point(306, 281);
            this.guna2BtnSetUp.Name = "guna2BtnSetUp";
            this.guna2BtnSetUp.Size = new System.Drawing.Size(180, 45);
            this.guna2BtnSetUp.TabIndex = 14;
            this.guna2BtnSetUp.Text = "Set Up";
            this.guna2BtnSetUp.Click += new System.EventHandler(this.guna2BtnSetUp_Click);
            // 
            // guna2CtmCkBxAutostart
            // 
            this.guna2CtmCkBxAutostart.Animated = true;
            this.guna2CtmCkBxAutostart.Checked = true;
            this.guna2CtmCkBxAutostart.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxAutostart.CheckedState.BorderRadius = 2;
            this.guna2CtmCkBxAutostart.CheckedState.BorderThickness = 0;
            this.guna2CtmCkBxAutostart.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxAutostart.Location = new System.Drawing.Point(466, 238);
            this.guna2CtmCkBxAutostart.Name = "guna2CtmCkBxAutostart";
            this.guna2CtmCkBxAutostart.Size = new System.Drawing.Size(20, 20);
            this.guna2CtmCkBxAutostart.TabIndex = 13;
            this.guna2CtmCkBxAutostart.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CtmCkBxAutostart.UncheckedState.BorderRadius = 2;
            this.guna2CtmCkBxAutostart.UncheckedState.BorderThickness = 0;
            this.guna2CtmCkBxAutostart.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // guna2CtmCkBxNotifications
            // 
            this.guna2CtmCkBxNotifications.Animated = true;
            this.guna2CtmCkBxNotifications.Checked = true;
            this.guna2CtmCkBxNotifications.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxNotifications.CheckedState.BorderRadius = 2;
            this.guna2CtmCkBxNotifications.CheckedState.BorderThickness = 0;
            this.guna2CtmCkBxNotifications.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CtmCkBxNotifications.Location = new System.Drawing.Point(466, 184);
            this.guna2CtmCkBxNotifications.Name = "guna2CtmCkBxNotifications";
            this.guna2CtmCkBxNotifications.Size = new System.Drawing.Size(20, 20);
            this.guna2CtmCkBxNotifications.TabIndex = 12;
            this.guna2CtmCkBxNotifications.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CtmCkBxNotifications.UncheckedState.BorderRadius = 2;
            this.guna2CtmCkBxNotifications.UncheckedState.BorderThickness = 0;
            this.guna2CtmCkBxNotifications.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            // 
            // lblTitleTwo
            // 
            this.lblTitleTwo.AutoSize = true;
            this.lblTitleTwo.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitleTwo.Location = new System.Drawing.Point(223, 17);
            this.lblTitleTwo.Name = "lblTitleTwo";
            this.lblTitleTwo.Size = new System.Drawing.Size(344, 117);
            this.lblTitleTwo.TabIndex = 10;
            this.lblTitleTwo.Text = "Get the most out of our app\r\nby enabling additional features\r\nfor an enhanced exp" +
    "erience.";
            this.lblTitleTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAutostart
            // 
            this.lblAutostart.AutoSize = true;
            this.lblAutostart.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAutostart.Location = new System.Drawing.Point(305, 235);
            this.lblAutostart.Name = "lblAutostart";
            this.lblAutostart.Size = new System.Drawing.Size(155, 23);
            this.lblAutostart.TabIndex = 9;
            this.lblAutostart.Text = "Run at system startup:";
            // 
            // lblNotifications
            // 
            this.lblNotifications.AutoSize = true;
            this.lblNotifications.Font = new System.Drawing.Font("Bahnschrift Condensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNotifications.Location = new System.Drawing.Point(305, 184);
            this.lblNotifications.Name = "lblNotifications";
            this.lblNotifications.Size = new System.Drawing.Size(138, 23);
            this.lblNotifications.TabIndex = 8;
            this.lblNotifications.Text = "Enable notifications:";
            // 
            // lblCountPageTwo
            // 
            this.lblCountPageTwo.AutoSize = true;
            this.lblCountPageTwo.Font = new System.Drawing.Font("Bahnschrift Condensed", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCountPageTwo.ForeColor = System.Drawing.Color.DimGray;
            this.lblCountPageTwo.Location = new System.Drawing.Point(751, 0);
            this.lblCountPageTwo.Name = "lblCountPageTwo";
            this.lblCountPageTwo.Size = new System.Drawing.Size(44, 33);
            this.lblCountPageTwo.TabIndex = 7;
            this.lblCountPageTwo.Text = "2/2";
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
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2TbCntrlAuth);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AuthorizationForm";
            this.Text = "Habit Manager | Authorization";
            this.guna2TbCntrlAuth.ResumeLayout(false);
            this.tbPgFirst.ResumeLayout(false);
            this.tbPgFirst.PerformLayout();
            this.tbPgSecond.ResumeLayout(false);
            this.tbPgSecond.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblUsername;
        private Guna.UI2.WinForms.Guna2TextBox guna2TxtBxName;
        private System.Windows.Forms.Label lblWelcomeAddition;
        private System.Windows.Forms.Label lblUnameHint;
        private Guna.UI2.WinForms.Guna2TabControl guna2TbCntrlAuth;
        private System.Windows.Forms.TabPage tbPgFirst;
        private System.Windows.Forms.TabPage tbPgSecond;
        private System.Windows.Forms.Label lblCountPageOne;
        private System.Windows.Forms.Label lblCountPageTwo;
        private System.Windows.Forms.Label lblAutostart;
        private System.Windows.Forms.Label lblNotifications;
        private Guna.UI2.WinForms.Guna2CustomCheckBox guna2CtmCkBxNotifications;
        private System.Windows.Forms.Label lblTitleTwo;
        private Guna.UI2.WinForms.Guna2CustomCheckBox guna2CtmCkBxAutostart;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSetUp;
        private Guna.UI2.WinForms.Guna2Button guna2BtnNext;
        private System.Windows.Forms.Label lblPassword;
        private Guna.UI2.WinForms.Guna2TextBox guna2TxtBxPassword;
        private System.Windows.Forms.Label lblRemember;
        private Guna.UI2.WinForms.Guna2CustomCheckBox guna2CtmCkBxRemember;
        private Guna.UI2.WinForms.Guna2Button guna2BtnBack;
        private System.Windows.Forms.Label lblPassHint;
        private Guna.UI2.WinForms.Guna2Button guna2BtnShowHide;
        private System.Windows.Forms.ToolTip tlTpMain;
    }
}