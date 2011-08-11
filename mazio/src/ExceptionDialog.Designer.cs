namespace mazio
{
    partial class ExceptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionDialog));
            this.textStackTrace = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkBugReport = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textStackTrace
            // 
            this.textStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textStackTrace.Location = new System.Drawing.Point(12, 30);
            this.textStackTrace.Margin = new System.Windows.Forms.Padding(0);
            this.textStackTrace.Multiline = true;
            this.textStackTrace.Name = "textStackTrace";
            this.textStackTrace.ReadOnly = true;
            this.textStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textStackTrace.Size = new System.Drawing.Size(452, 330);
            this.textStackTrace.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(201, 372);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "I am sorry, but Mazio has just crashed. Please report the bug ";
            // 
            // linkBugReport
            // 
            this.linkBugReport.AutoSize = true;
            this.linkBugReport.Location = new System.Drawing.Point(302, 9);
            this.linkBugReport.Name = "linkBugReport";
            this.linkBugReport.Size = new System.Drawing.Size(28, 13);
            this.linkBugReport.TabIndex = 3;
            this.linkBugReport.TabStop = true;
            this.linkBugReport.Text = "here";
            this.linkBugReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBugReport_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "and paste the stack trace";
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 407);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkBugReport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textStackTrace);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionDialog";
            this.Text = "ExceptionDialog";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.exceptionDialogKeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textStackTrace;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkBugReport;
        private System.Windows.Forms.Label label2;
    }
}