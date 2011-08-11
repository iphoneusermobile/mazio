namespace mazio.uploaders
{
    partial class JiraUpload
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
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.uploadLog = new System.Windows.Forms.RichTextBox();
            this.comboEntryName = new System.Windows.Forms.ComboBox();
            this.textLogin = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelServerUrl = new System.Windows.Forms.Label();
            this.labelIssueNumber = new System.Windows.Forms.Label();
            this.checkStorePassword = new System.Windows.Forms.CheckBox();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.groupBoxComment = new System.Windows.Forms.GroupBox();
            this.commentBox = new System.Windows.Forms.TextBox();
            this.groupBoxAccounts = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelEntryName = new System.Windows.Forms.Label();
            this.textServerUrl = new System.Windows.Forms.TextBox();
            this.textIssueNumber = new System.Windows.Forms.TextBox();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxComment.SuspendLayout();
            this.groupBoxAccounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.FlatAppearance.BorderSize = 0;
            this.buttonUpload.Location = new System.Drawing.Point(542, 382);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 1;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.Location = new System.Drawing.Point(623, 382);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // uploadLog
            // 
            this.uploadLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uploadLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uploadLog.Location = new System.Drawing.Point(3, 16);
            this.uploadLog.Name = "uploadLog";
            this.uploadLog.ReadOnly = true;
            this.uploadLog.Size = new System.Drawing.Size(679, 38);
            this.uploadLog.TabIndex = 2;
            this.uploadLog.TabStop = false;
            this.uploadLog.Text = "";
            this.uploadLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.uploadLog_LinkClicked);
            // 
            // comboEntryName
            // 
            this.comboEntryName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboEntryName.FormattingEnabled = true;
            this.comboEntryName.Location = new System.Drawing.Point(113, 20);
            this.comboEntryName.Name = "comboEntryName";
            this.comboEntryName.Size = new System.Drawing.Size(170, 21);
            this.comboEntryName.TabIndex = 0;
            this.comboEntryName.TextChanged += new System.EventHandler(this.comboEntryName_TextChanged);
            // 
            // textLogin
            // 
            this.textLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textLogin.Location = new System.Drawing.Point(539, 20);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(127, 20);
            this.textLogin.TabIndex = 2;
            this.textLogin.TextChanged += new System.EventHandler(this.userName_TextChanged);
            // 
            // textPassword
            // 
            this.textPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPassword.Location = new System.Drawing.Point(539, 46);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(127, 20);
            this.textPassword.TabIndex = 3;
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(473, 24);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(60, 13);
            this.labelUserName.TabIndex = 7;
            this.labelUserName.Text = "User Name";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(473, 50);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 8;
            this.labelPassword.Text = "Password";
            // 
            // labelServerUrl
            // 
            this.labelServerUrl.AutoSize = true;
            this.labelServerUrl.Location = new System.Drawing.Point(18, 50);
            this.labelServerUrl.Name = "labelServerUrl";
            this.labelServerUrl.Size = new System.Drawing.Size(89, 13);
            this.labelServerUrl.TabIndex = 9;
            this.labelServerUrl.Text = "JIRA Server URL";
            // 
            // labelIssueNumber
            // 
            this.labelIssueNumber.AutoSize = true;
            this.labelIssueNumber.Location = new System.Drawing.Point(10, 188);
            this.labelIssueNumber.Name = "labelIssueNumber";
            this.labelIssueNumber.Size = new System.Drawing.Size(72, 13);
            this.labelIssueNumber.TabIndex = 10;
            this.labelIssueNumber.Text = "Issue Number";
            // 
            // checkStorePassword
            // 
            this.checkStorePassword.AutoSize = true;
            this.checkStorePassword.Checked = true;
            this.checkStorePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkStorePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkStorePassword.Location = new System.Drawing.Point(539, 72);
            this.checkStorePassword.Name = "checkStorePassword";
            this.checkStorePassword.Size = new System.Drawing.Size(97, 17);
            this.checkStorePassword.TabIndex = 4;
            this.checkStorePassword.Text = "Store Password";
            this.checkStorePassword.UseVisualStyleBackColor = true;
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLog.Controls.Add(this.uploadLog);
            this.groupBoxLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxLog.Location = new System.Drawing.Point(12, 314);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(685, 57);
            this.groupBoxLog.TabIndex = 12;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Upload Log";
            // 
            // groupBoxComment
            // 
            this.groupBoxComment.Controls.Add(this.commentBox);
            this.groupBoxComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxComment.Location = new System.Drawing.Point(12, 212);
            this.groupBoxComment.Name = "groupBoxComment";
            this.groupBoxComment.Size = new System.Drawing.Size(685, 99);
            this.groupBoxComment.TabIndex = 13;
            this.groupBoxComment.TabStop = false;
            this.groupBoxComment.Text = "Comment";
            // 
            // commentBox
            // 
            this.commentBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commentBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commentBox.Location = new System.Drawing.Point(3, 16);
            this.commentBox.Multiline = true;
            this.commentBox.Name = "commentBox";
            this.commentBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commentBox.Size = new System.Drawing.Size(679, 80);
            this.commentBox.TabIndex = 0;
            // 
            // groupBoxAccounts
            // 
            this.groupBoxAccounts.Controls.Add(this.btnDelete);
            this.groupBoxAccounts.Controls.Add(this.btnSave);
            this.groupBoxAccounts.Controls.Add(this.labelEntryName);
            this.groupBoxAccounts.Controls.Add(this.textServerUrl);
            this.groupBoxAccounts.Controls.Add(this.checkStorePassword);
            this.groupBoxAccounts.Controls.Add(this.labelServerUrl);
            this.groupBoxAccounts.Controls.Add(this.labelPassword);
            this.groupBoxAccounts.Controls.Add(this.labelUserName);
            this.groupBoxAccounts.Controls.Add(this.textPassword);
            this.groupBoxAccounts.Controls.Add(this.textLogin);
            this.groupBoxAccounts.Controls.Add(this.comboEntryName);
            this.groupBoxAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxAccounts.Location = new System.Drawing.Point(13, 8);
            this.groupBoxAccounts.Name = "groupBoxAccounts";
            this.groupBoxAccounts.Size = new System.Drawing.Size(684, 166);
            this.groupBoxAccounts.TabIndex = 14;
            this.groupBoxAccounts.TabStop = false;
            this.groupBoxAccounts.Text = "JIRA Accounts";
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Location = new System.Drawing.Point(537, 124);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(129, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete Account Info";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Location = new System.Drawing.Point(537, 95);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(129, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save Account Info";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelEntryName
            // 
            this.labelEntryName.AutoSize = true;
            this.labelEntryName.Location = new System.Drawing.Point(18, 24);
            this.labelEntryName.Name = "labelEntryName";
            this.labelEntryName.Size = new System.Drawing.Size(62, 13);
            this.labelEntryName.TabIndex = 13;
            this.labelEntryName.Text = "Entry Name";
            // 
            // textServerUrl
            // 
            this.textServerUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textServerUrl.Location = new System.Drawing.Point(113, 47);
            this.textServerUrl.Name = "textServerUrl";
            this.textServerUrl.Size = new System.Drawing.Size(336, 20);
            this.textServerUrl.TabIndex = 1;
            this.textServerUrl.TextChanged += new System.EventHandler(this.textServerUrl_TextChanged);
            // 
            // textIssueNumber
            // 
            this.textIssueNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textIssueNumber.Location = new System.Drawing.Point(126, 185);
            this.textIssueNumber.Name = "textIssueNumber";
            this.textIssueNumber.Size = new System.Drawing.Size(127, 20);
            this.textIssueNumber.TabIndex = 15;
            this.textIssueNumber.TextChanged += new System.EventHandler(this.textIssueNumber_TextChanged);
            // 
            // JiraUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 417);
            this.ControlBox = false;
            this.Controls.Add(this.textIssueNumber);
            this.Controls.Add(this.groupBoxAccounts);
            this.Controls.Add(this.groupBoxComment);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.labelIssueNumber);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "JiraUpload";
            this.ShowInTaskbar = false;
            this.Text = "Upload to JIRA";
            this.Load += new System.EventHandler(this.jiraUploadLoad);
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxComment.ResumeLayout(false);
            this.groupBoxComment.PerformLayout();
            this.groupBoxAccounts.ResumeLayout(false);
            this.groupBoxAccounts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RichTextBox uploadLog;
        private System.Windows.Forms.ComboBox comboEntryName;
        private System.Windows.Forms.TextBox textLogin;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelServerUrl;
        private System.Windows.Forms.Label labelIssueNumber;
        private System.Windows.Forms.CheckBox checkStorePassword;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.GroupBox groupBoxComment;
        private System.Windows.Forms.TextBox commentBox;
        private System.Windows.Forms.GroupBox groupBoxAccounts;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelEntryName;
        private System.Windows.Forms.TextBox textServerUrl;
        private System.Windows.Forms.TextBox textIssueNumber;
    }
}