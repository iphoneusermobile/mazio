namespace mazio
{
    partial class ConfigDialog
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
            this.tabbedPanelSettings = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.checkShowMagnifier = new System.Windows.Forms.CheckBox();
            this.checkLimitGrabByActiveScreen = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericZoom = new System.Windows.Forms.NumericUpDown();
            this.checkShowUsageHints = new System.Windows.Forms.CheckBox();
            this.labelGrabMargin = new System.Windows.Forms.Label();
            this.trackGrabMargin = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tabJpeg = new System.Windows.Forms.TabPage();
            this.labelJpegQuality = new System.Windows.Forms.Label();
            this.trackJpegQuality = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMail = new System.Windows.Forms.TabPage();
            this.checkStorePassword = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkSSL = new System.Windows.Forms.CheckBox();
            this.textSMTPPort = new System.Windows.Forms.TextBox();
            this.textSMTPServer = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textUserName = new System.Windows.Forms.TextBox();
            this.tabbedPanelSettings.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGrabMargin)).BeginInit();
            this.tabJpeg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackJpegQuality)).BeginInit();
            this.tabMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabbedPanelSettings
            // 
            this.tabbedPanelSettings.Controls.Add(this.tabGeneral);
            this.tabbedPanelSettings.Controls.Add(this.tabJpeg);
            this.tabbedPanelSettings.Controls.Add(this.tabMail);
            this.tabbedPanelSettings.Location = new System.Drawing.Point(12, 12);
            this.tabbedPanelSettings.Name = "tabbedPanelSettings";
            this.tabbedPanelSettings.SelectedIndex = 0;
            this.tabbedPanelSettings.Size = new System.Drawing.Size(373, 330);
            this.tabbedPanelSettings.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.checkShowMagnifier);
            this.tabGeneral.Controls.Add(this.checkLimitGrabByActiveScreen);
            this.tabGeneral.Controls.Add(this.label8);
            this.tabGeneral.Controls.Add(this.label7);
            this.tabGeneral.Controls.Add(this.numericZoom);
            this.tabGeneral.Controls.Add(this.checkShowUsageHints);
            this.tabGeneral.Controls.Add(this.labelGrabMargin);
            this.tabGeneral.Controls.Add(this.trackGrabMargin);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(365, 304);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General Settings";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // checkShowMagnifier
            // 
            this.checkShowMagnifier.AutoSize = true;
            this.checkShowMagnifier.Checked = true;
            this.checkShowMagnifier.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowMagnifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkShowMagnifier.Location = new System.Drawing.Point(50, 161);
            this.checkShowMagnifier.Name = "checkShowMagnifier";
            this.checkShowMagnifier.Size = new System.Drawing.Size(209, 17);
            this.checkShowMagnifier.TabIndex = 8;
            this.checkShowMagnifier.Text = "Show magnifier when grabbing desktop";
            this.checkShowMagnifier.UseVisualStyleBackColor = true;
            this.checkShowMagnifier.CheckedChanged += new System.EventHandler(this.checkShowMagnifier_CheckedChanged);
            // 
            // checkLimitGrabByActiveScreen
            // 
            this.checkLimitGrabByActiveScreen.AutoSize = true;
            this.checkLimitGrabByActiveScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkLimitGrabByActiveScreen.Location = new System.Drawing.Point(50, 124);
            this.checkLimitGrabByActiveScreen.Name = "checkLimitGrabByActiveScreen";
            this.checkLimitGrabByActiveScreen.Size = new System.Drawing.Size(171, 17);
            this.checkLimitGrabByActiveScreen.TabIndex = 7;
            this.checkLimitGrabByActiveScreen.Text = "Limit grab area to active screen";
            this.checkLimitGrabByActiveScreen.UseVisualStyleBackColor = true;
            this.checkLimitGrabByActiveScreen.CheckedChanged += new System.EventHandler(this.checkLimitGrabByActiveScreen_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(277, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Default Magnifier Zoom";
            // 
            // numericZoom
            // 
            this.numericZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericZoom.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericZoom.Location = new System.Drawing.Point(218, 89);
            this.numericZoom.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericZoom.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericZoom.Name = "numericZoom";
            this.numericZoom.Size = new System.Drawing.Size(53, 20);
            this.numericZoom.TabIndex = 4;
            this.numericZoom.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericZoom.ValueChanged += new System.EventHandler(this.numericZoom_ValueChanged);
            // 
            // checkShowUsageHints
            // 
            this.checkShowUsageHints.AutoSize = true;
            this.checkShowUsageHints.Checked = true;
            this.checkShowUsageHints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShowUsageHints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkShowUsageHints.Location = new System.Drawing.Point(50, 198);
            this.checkShowUsageHints.Name = "checkShowUsageHints";
            this.checkShowUsageHints.Size = new System.Drawing.Size(111, 17);
            this.checkShowUsageHints.TabIndex = 3;
            this.checkShowUsageHints.Text = "Show Usage Hints";
            this.checkShowUsageHints.UseVisualStyleBackColor = true;
            this.checkShowUsageHints.CheckedChanged += new System.EventHandler(this.checkShowUsageHints_CheckedChanged);
            // 
            // labelGrabMargin
            // 
            this.labelGrabMargin.AutoSize = true;
            this.labelGrabMargin.Enabled = false;
            this.labelGrabMargin.Location = new System.Drawing.Point(283, 77);
            this.labelGrabMargin.Name = "labelGrabMargin";
            this.labelGrabMargin.Size = new System.Drawing.Size(35, 13);
            this.labelGrabMargin.TabIndex = 2;
            this.labelGrabMargin.Text = "label3";
            this.labelGrabMargin.Visible = false;
            // 
            // trackGrabMargin
            // 
            this.trackGrabMargin.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackGrabMargin.Enabled = false;
            this.trackGrabMargin.Location = new System.Drawing.Point(134, 67);
            this.trackGrabMargin.Name = "trackGrabMargin";
            this.trackGrabMargin.Size = new System.Drawing.Size(143, 45);
            this.trackGrabMargin.TabIndex = 1;
            this.trackGrabMargin.Visible = false;
            this.trackGrabMargin.ValueChanged += new System.EventHandler(this.trackGrabMargin_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(47, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Grab Margin";
            this.label2.Visible = false;
            // 
            // tabJpeg
            // 
            this.tabJpeg.Controls.Add(this.labelJpegQuality);
            this.tabJpeg.Controls.Add(this.trackJpegQuality);
            this.tabJpeg.Controls.Add(this.label1);
            this.tabJpeg.Location = new System.Drawing.Point(4, 22);
            this.tabJpeg.Name = "tabJpeg";
            this.tabJpeg.Padding = new System.Windows.Forms.Padding(3);
            this.tabJpeg.Size = new System.Drawing.Size(365, 304);
            this.tabJpeg.TabIndex = 1;
            this.tabJpeg.Text = "JPEG Settings";
            this.tabJpeg.UseVisualStyleBackColor = true;
            // 
            // labelJpegQuality
            // 
            this.labelJpegQuality.AutoSize = true;
            this.labelJpegQuality.Location = new System.Drawing.Point(277, 141);
            this.labelJpegQuality.Name = "labelJpegQuality";
            this.labelJpegQuality.Size = new System.Drawing.Size(35, 13);
            this.labelJpegQuality.TabIndex = 4;
            this.labelJpegQuality.Text = "label4";
            // 
            // trackJpegQuality
            // 
            this.trackJpegQuality.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trackJpegQuality.Location = new System.Drawing.Point(128, 131);
            this.trackJpegQuality.Name = "trackJpegQuality";
            this.trackJpegQuality.Size = new System.Drawing.Size(143, 45);
            this.trackJpegQuality.TabIndex = 3;
            this.trackJpegQuality.TickFrequency = 10;
            this.trackJpegQuality.ValueChanged += new System.EventHandler(this.trackJpegQuality_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "JPEG Quality";
            // 
            // tabMail
            // 
            this.tabMail.Controls.Add(this.checkStorePassword);
            this.tabMail.Controls.Add(this.label6);
            this.tabMail.Controls.Add(this.label5);
            this.tabMail.Controls.Add(this.label4);
            this.tabMail.Controls.Add(this.label3);
            this.tabMail.Controls.Add(this.checkSSL);
            this.tabMail.Controls.Add(this.textSMTPPort);
            this.tabMail.Controls.Add(this.textSMTPServer);
            this.tabMail.Controls.Add(this.textPassword);
            this.tabMail.Controls.Add(this.textUserName);
            this.tabMail.Location = new System.Drawing.Point(4, 22);
            this.tabMail.Name = "tabMail";
            this.tabMail.Padding = new System.Windows.Forms.Padding(3);
            this.tabMail.Size = new System.Drawing.Size(365, 304);
            this.tabMail.TabIndex = 2;
            this.tabMail.Text = "Mail Settings";
            this.tabMail.UseVisualStyleBackColor = true;
            // 
            // checkStorePassword
            // 
            this.checkStorePassword.AutoSize = true;
            this.checkStorePassword.Checked = true;
            this.checkStorePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkStorePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkStorePassword.Location = new System.Drawing.Point(272, 101);
            this.checkStorePassword.Name = "checkStorePassword";
            this.checkStorePassword.Size = new System.Drawing.Size(48, 17);
            this.checkStorePassword.TabIndex = 9;
            this.checkStorePassword.Text = "Store";
            this.checkStorePassword.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "SMTP Server";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User Name";
            // 
            // checkSSL
            // 
            this.checkSSL.AutoSize = true;
            this.checkSSL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkSSL.Location = new System.Drawing.Point(125, 232);
            this.checkSSL.Name = "checkSSL";
            this.checkSSL.Size = new System.Drawing.Size(136, 17);
            this.checkSSL.TabIndex = 4;
            this.checkSSL.Text = "Use Secure Connection";
            this.checkSSL.UseVisualStyleBackColor = true;
            this.checkSSL.CheckedChanged += new System.EventHandler(this.checkSslCheckedChanged);
            // 
            // textSMTPPort
            // 
            this.textSMTPPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textSMTPPort.Location = new System.Drawing.Point(125, 191);
            this.textSMTPPort.Name = "textSMTPPort";
            this.textSMTPPort.Size = new System.Drawing.Size(68, 20);
            this.textSMTPPort.TabIndex = 3;
            this.textSMTPPort.TextChanged += new System.EventHandler(this.textSmtpPortTextChanged);
            this.textSMTPPort.Validating += new System.ComponentModel.CancelEventHandler(this.textSmtpPortValidating);
            // 
            // textSMTPServer
            // 
            this.textSMTPServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textSMTPServer.Location = new System.Drawing.Point(125, 145);
            this.textSMTPServer.Name = "textSMTPServer";
            this.textSMTPServer.Size = new System.Drawing.Size(206, 20);
            this.textSMTPServer.TabIndex = 2;
            this.textSMTPServer.TextChanged += new System.EventHandler(this.textSmtpServerTextChanged);
            // 
            // textPassword
            // 
            this.textPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPassword.Location = new System.Drawing.Point(125, 100);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(128, 20);
            this.textPassword.TabIndex = 1;
            this.textPassword.UseSystemPasswordChar = true;
            this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
            // 
            // textUserName
            // 
            this.textUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textUserName.Location = new System.Drawing.Point(125, 55);
            this.textUserName.Name = "textUserName";
            this.textUserName.Size = new System.Drawing.Size(128, 20);
            this.textUserName.TabIndex = 0;
            this.textUserName.TextChanged += new System.EventHandler(this.textUserName_TextChanged);
            // 
            // ConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 354);
            this.Controls.Add(this.tabbedPanelSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mazio Configuration";
            this.TopMost = true;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.configDialogKeyPress);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.configDialogFormClosing);
            this.tabbedPanelSettings.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackGrabMargin)).EndInit();
            this.tabJpeg.ResumeLayout(false);
            this.tabJpeg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackJpegQuality)).EndInit();
            this.tabMail.ResumeLayout(false);
            this.tabMail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabbedPanelSettings;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabJpeg;
        private System.Windows.Forms.TrackBar trackJpegQuality;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelGrabMargin;
        private System.Windows.Forms.TrackBar trackGrabMargin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelJpegQuality;
        private System.Windows.Forms.TabPage tabMail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkSSL;
        private System.Windows.Forms.TextBox textSMTPPort;
        private System.Windows.Forms.TextBox textSMTPServer;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textUserName;
        private System.Windows.Forms.CheckBox checkStorePassword;
        private System.Windows.Forms.CheckBox checkShowUsageHints;
        private System.Windows.Forms.NumericUpDown numericZoom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkLimitGrabByActiveScreen;
        private System.Windows.Forms.CheckBox checkShowMagnifier;
    }
}
