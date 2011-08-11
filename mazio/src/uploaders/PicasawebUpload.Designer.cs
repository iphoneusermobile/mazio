namespace mazio.uploaders
{
    partial class PicasawebUpload
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
            this.uploadLog = new System.Windows.Forms.RichTextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.listAlbums = new System.Windows.Forms.ListBox();
            this.buttonGetAlbumList = new System.Windows.Forms.Button();
            this.checkUploadToDropbox = new System.Windows.Forms.CheckBox();
            this.textComment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkStorePassword = new System.Windows.Forms.CheckBox();
            this.comboLogin = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadLog
            // 
            this.uploadLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uploadLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uploadLog.Location = new System.Drawing.Point(3, 16);
            this.uploadLog.Name = "uploadLog";
            this.uploadLog.ReadOnly = true;
            this.uploadLog.Size = new System.Drawing.Size(709, 38);
            this.uploadLog.TabIndex = 0;
            this.uploadLog.TabStop = false;
            this.uploadLog.Text = "";
            this.uploadLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.uploadLog_LinkClicked);
            // 
            // buttonOk
            // 
            this.buttonOk.FlatAppearance.BorderSize = 0;
            this.buttonOk.Location = new System.Drawing.Point(569, 387);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Upload";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.Location = new System.Drawing.Point(650, 387);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textPassword
            // 
            this.textPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPassword.Location = new System.Drawing.Point(178, 53);
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.Size = new System.Drawing.Size(145, 20);
            this.textPassword.TabIndex = 1;
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // listAlbums
            // 
            this.listAlbums.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listAlbums.FormattingEnabled = true;
            this.listAlbums.Location = new System.Drawing.Point(19, 19);
            this.listAlbums.Name = "listAlbums";
            this.listAlbums.Size = new System.Drawing.Size(315, 67);
            this.listAlbums.TabIndex = 5;
            // 
            // buttonGetAlbumList
            // 
            this.buttonGetAlbumList.FlatAppearance.BorderSize = 0;
            this.buttonGetAlbumList.Location = new System.Drawing.Point(19, 107);
            this.buttonGetAlbumList.Name = "buttonGetAlbumList";
            this.buttonGetAlbumList.Size = new System.Drawing.Size(315, 23);
            this.buttonGetAlbumList.TabIndex = 0;
            this.buttonGetAlbumList.Text = "Get Album List";
            this.buttonGetAlbumList.UseVisualStyleBackColor = true;
            this.buttonGetAlbumList.Click += new System.EventHandler(this.buttonGetAlbumList_Click);
            // 
            // checkUploadToDropbox
            // 
            this.checkUploadToDropbox.AutoSize = true;
            this.checkUploadToDropbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUploadToDropbox.Location = new System.Drawing.Point(19, 148);
            this.checkUploadToDropbox.Name = "checkUploadToDropbox";
            this.checkUploadToDropbox.Size = new System.Drawing.Size(120, 17);
            this.checkUploadToDropbox.TabIndex = 1;
            this.checkUploadToDropbox.Text = "Upload To Drop Box";
            this.checkUploadToDropbox.UseVisualStyleBackColor = true;
            this.checkUploadToDropbox.CheckedChanged += new System.EventHandler(this.checkUploadToDropbox_CheckedChanged);
            // 
            // textComment
            // 
            this.textComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textComment.Location = new System.Drawing.Point(3, 16);
            this.textComment.Multiline = true;
            this.textComment.Name = "textComment";
            this.textComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textComment.Size = new System.Drawing.Size(709, 80);
            this.textComment.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "User ID (e.g. \"joe@gmail.com\")";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textComment);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(10, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(715, 99);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uploadLog);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(10, 308);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(715, 57);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Upload Log";
            // 
            // checkStorePassword
            // 
            this.checkStorePassword.AutoSize = true;
            this.checkStorePassword.Checked = true;
            this.checkStorePassword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkStorePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkStorePassword.Location = new System.Drawing.Point(178, 79);
            this.checkStorePassword.Name = "checkStorePassword";
            this.checkStorePassword.Size = new System.Drawing.Size(97, 17);
            this.checkStorePassword.TabIndex = 2;
            this.checkStorePassword.Text = "Store Password";
            this.checkStorePassword.UseVisualStyleBackColor = true;
            // 
            // comboLogin
            // 
            this.comboLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboLogin.FormattingEnabled = true;
            this.comboLogin.Location = new System.Drawing.Point(178, 23);
            this.comboLogin.Name = "comboLogin";
            this.comboLogin.Size = new System.Drawing.Size(145, 21);
            this.comboLogin.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Location = new System.Drawing.Point(178, 107);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Account Info";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.comboLogin);
            this.groupBox3.Controls.Add(this.checkStorePassword);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textPassword);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(10, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(351, 187);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Picasaweb Accounts";
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Location = new System.Drawing.Point(178, 142);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(145, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete Account Info";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkUploadToDropbox);
            this.groupBox4.Controls.Add(this.buttonGetAlbumList);
            this.groupBox4.Controls.Add(this.listAlbums);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox4.Location = new System.Drawing.Point(367, 10);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(358, 187);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Album List";
            // 
            // PicasawebUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 422);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PicasawebUpload";
            this.ShowInTaskbar = false;
            this.Text = "Upload to Picasaweb";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox uploadLog;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.ListBox listAlbums;
        private System.Windows.Forms.Button buttonGetAlbumList;
        private System.Windows.Forms.CheckBox checkUploadToDropbox;
        private System.Windows.Forms.TextBox textComment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkStorePassword;
        private System.Windows.Forms.ComboBox comboLogin;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}