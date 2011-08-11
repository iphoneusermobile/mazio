namespace mazio.uploaders
{
    partial class SkitchUpload
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
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textDescription = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkPublic = new System.Windows.Forms.CheckBox();
            this.checkTwitter = new System.Windows.Forms.CheckBox();
            this.comboMaildrops = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadLog
            // 
            this.uploadLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uploadLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uploadLog.Location = new System.Drawing.Point(3, 16);
            this.uploadLog.Name = "uploadLog";
            this.uploadLog.ReadOnly = true;
            this.uploadLog.Size = new System.Drawing.Size(527, 38);
            this.uploadLog.TabIndex = 0;
            this.uploadLog.Text = "";
            this.uploadLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.uploadLog_LinkClicked);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Enabled = false;
            this.buttonUpload.FlatAppearance.BorderSize = 0;
            this.buttonUpload.Location = new System.Drawing.Point(383, 223);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 3;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.Location = new System.Drawing.Point(464, 223);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Skitch MailDrop";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "@skitch.com";
            // 
            // textDescription
            // 
            this.textDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textDescription.Location = new System.Drawing.Point(3, 16);
            this.textDescription.Multiline = true;
            this.textDescription.Name = "textDescription";
            this.textDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textDescription.Size = new System.Drawing.Size(527, 80);
            this.textDescription.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textDescription);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(6, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 99);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uploadLog);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(6, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(533, 57);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Upload Log";
            // 
            // checkPublic
            // 
            this.checkPublic.AutoSize = true;
            this.checkPublic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkPublic.Location = new System.Drawing.Point(339, 19);
            this.checkPublic.Name = "checkPublic";
            this.checkPublic.Size = new System.Drawing.Size(82, 17);
            this.checkPublic.TabIndex = 1;
            this.checkPublic.Text = "Make Public";
            this.checkPublic.UseVisualStyleBackColor = true;
            // 
            // checkTwitter
            // 
            this.checkTwitter.AutoSize = true;
            this.checkTwitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkTwitter.Location = new System.Drawing.Point(438, 19);
            this.checkTwitter.Name = "checkTwitter";
            this.checkTwitter.Size = new System.Drawing.Size(95, 17);
            this.checkTwitter.TabIndex = 2;
            this.checkTwitter.Text = "Send to Twitter";
            this.checkTwitter.UseVisualStyleBackColor = true;
            // 
            // comboMaildrops
            // 
            this.comboMaildrops.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboMaildrops.FormattingEnabled = true;
            this.comboMaildrops.Location = new System.Drawing.Point(100, 15);
            this.comboMaildrops.Name = "comboMaildrops";
            this.comboMaildrops.Size = new System.Drawing.Size(104, 21);
            this.comboMaildrops.TabIndex = 0;
            // 
            // SkitchUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 258);
            this.ControlBox = false;
            this.Controls.Add(this.comboMaildrops);
            this.Controls.Add(this.checkTwitter);
            this.Controls.Add(this.checkPublic);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "SkitchUpload";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Upload To Skitch";
            this.Activated += new System.EventHandler(this.SkitchUpload_Activated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox uploadLog;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textDescription;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkPublic;
        private System.Windows.Forms.CheckBox checkTwitter;
        private System.Windows.Forms.ComboBox comboMaildrops;

    }
}