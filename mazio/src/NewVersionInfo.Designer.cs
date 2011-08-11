namespace mazio
{
    partial class NewVersionInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewVersionInfo));
            this.web = new System.Windows.Forms.WebBrowser();
            this.buttonGetIt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // web
            // 
            this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.web.Location = new System.Drawing.Point(19, 19);
            this.web.Margin = new System.Windows.Forms.Padding(10);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.ScrollBarsEnabled = false;
            this.web.Size = new System.Drawing.Size(664, 254);
            this.web.TabIndex = 0;
            // 
            // buttonGetIt
            // 
            this.buttonGetIt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonGetIt.FlatAppearance.BorderSize = 0;
            this.buttonGetIt.Location = new System.Drawing.Point(247, 300);
            this.buttonGetIt.Name = "buttonGetIt";
            this.buttonGetIt.Size = new System.Drawing.Size(209, 23);
            this.buttonGetIt.TabIndex = 1;
            this.buttonGetIt.Text = "Get Mazio .NET, version XX.XX.XX";
            this.buttonGetIt.UseVisualStyleBackColor = true;
            this.buttonGetIt.Click += new System.EventHandler(this.buttonGetIt_Click);
            // 
            // NewVersionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 344);
            this.Controls.Add(this.buttonGetIt);
            this.Controls.Add(this.web);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewVersionInfo";
            this.ShowInTaskbar = false;
            this.Text = "New Version Information";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.Button buttonGetIt;
    }
}