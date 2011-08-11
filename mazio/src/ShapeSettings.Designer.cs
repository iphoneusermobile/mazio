namespace mazio
{
    partial class ShapeSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeSettings));
            this.basePanel = new System.Windows.Forms.Panel();
            this.labelEmpty = new System.Windows.Forms.Label();
            this.basePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // basePanel
            // 
            this.basePanel.Controls.Add(this.labelEmpty);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(177, 273);
            this.basePanel.TabIndex = 0;
            // 
            // labelEmpty
            // 
            this.labelEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEmpty.Location = new System.Drawing.Point(0, 0);
            this.labelEmpty.Name = "labelEmpty";
            this.labelEmpty.Size = new System.Drawing.Size(177, 273);
            this.labelEmpty.TabIndex = 0;
            this.labelEmpty.Text = "Selected tool has no settings or no tool selected";
            this.labelEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShapeSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(177, 273);
            this.Controls.Add(this.basePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShapeSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tool Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.shapeSettingsLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.shapeSettingsFormClosed);
            this.basePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel basePanel;
        private System.Windows.Forms.Label labelEmpty;
    }
}