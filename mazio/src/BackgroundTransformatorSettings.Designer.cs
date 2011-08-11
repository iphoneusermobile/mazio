namespace mazio {
    partial class BackgroundTransformatorSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgroundTransformatorSettings));
            this.trackHorizontalRotationOffset = new System.Windows.Forms.TrackBar();
            this.trackRotationAngle = new System.Windows.Forms.TrackBar();
            this.trackVerticalRotationOffset = new System.Windows.Forms.TrackBar();
            this.trackPerspective = new System.Windows.Forms.TrackBar();
            this.buttonResetPerspective = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkDrawPivotPoint = new System.Windows.Forms.CheckBox();
            this.buttonResetRotation = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackHorizontalRotationOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRotationAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackVerticalRotationOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPerspective)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackHorizontalRotationOffset
            // 
            this.trackHorizontalRotationOffset.Location = new System.Drawing.Point(140, 62);
            this.trackHorizontalRotationOffset.Maximum = 100;
            this.trackHorizontalRotationOffset.Minimum = -100;
            this.trackHorizontalRotationOffset.Name = "trackHorizontalRotationOffset";
            this.trackHorizontalRotationOffset.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackHorizontalRotationOffset.Size = new System.Drawing.Size(45, 115);
            this.trackHorizontalRotationOffset.TabIndex = 0;
            this.trackHorizontalRotationOffset.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackHorizontalRotationOffset.ValueChanged += new System.EventHandler(this.trackHorizontalRotationOffset_ValueChanged);
            // 
            // trackRotationAngle
            // 
            this.trackRotationAngle.Location = new System.Drawing.Point(13, 46);
            this.trackRotationAngle.Maximum = 180;
            this.trackRotationAngle.Minimum = -180;
            this.trackRotationAngle.Name = "trackRotationAngle";
            this.trackRotationAngle.Size = new System.Drawing.Size(104, 45);
            this.trackRotationAngle.TabIndex = 1;
            this.trackRotationAngle.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackRotationAngle.ValueChanged += new System.EventHandler(this.trackRotationAngle_ValueChanged);
            // 
            // trackVerticalRotationOffset
            // 
            this.trackVerticalRotationOffset.Location = new System.Drawing.Point(140, 46);
            this.trackVerticalRotationOffset.Maximum = 100;
            this.trackVerticalRotationOffset.Minimum = -100;
            this.trackVerticalRotationOffset.Name = "trackVerticalRotationOffset";
            this.trackVerticalRotationOffset.Size = new System.Drawing.Size(154, 45);
            this.trackVerticalRotationOffset.TabIndex = 2;
            this.trackVerticalRotationOffset.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackVerticalRotationOffset.ValueChanged += new System.EventHandler(this.trackVerticalRotationOffset_ValueChanged);
            // 
            // trackPerspective
            // 
            this.trackPerspective.Location = new System.Drawing.Point(59, 19);
            this.trackPerspective.Maximum = 100;
            this.trackPerspective.Minimum = -100;
            this.trackPerspective.Name = "trackPerspective";
            this.trackPerspective.Size = new System.Drawing.Size(235, 45);
            this.trackPerspective.TabIndex = 7;
            this.trackPerspective.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackPerspective.ValueChanged += new System.EventHandler(this.trackPerspective_ValueChanged);
            // 
            // buttonResetPerspective
            // 
            this.buttonResetPerspective.Location = new System.Drawing.Point(13, 66);
            this.buttonResetPerspective.Name = "buttonResetPerspective";
            this.buttonResetPerspective.Size = new System.Drawing.Size(109, 23);
            this.buttonResetPerspective.TabIndex = 8;
            this.buttonResetPerspective.Text = "Reset Perspective";
            this.buttonResetPerspective.UseVisualStyleBackColor = true;
            this.buttonResetPerspective.Click += new System.EventHandler(this.buttonResetPerspective_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Rotation Angle";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Angle";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkDrawPivotPoint);
            this.groupBox1.Controls.Add(this.buttonResetRotation);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.trackHorizontalRotationOffset);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.trackVerticalRotationOffset);
            this.groupBox1.Controls.Add(this.trackRotationAngle);
            this.groupBox1.Location = new System.Drawing.Point(12, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 183);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rotation";
            // 
            // checkDrawPivotPoint
            // 
            this.checkDrawPivotPoint.AutoSize = true;
            this.checkDrawPivotPoint.Location = new System.Drawing.Point(189, 149);
            this.checkDrawPivotPoint.Name = "checkDrawPivotPoint";
            this.checkDrawPivotPoint.Size = new System.Drawing.Size(105, 17);
            this.checkDrawPivotPoint.TabIndex = 17;
            this.checkDrawPivotPoint.Text = "Draw Pivot Point";
            this.checkDrawPivotPoint.UseVisualStyleBackColor = true;
            this.checkDrawPivotPoint.CheckedChanged += new System.EventHandler(this.checkDrawPivotPoint_CheckedChanged);
            // 
            // buttonResetRotation
            // 
            this.buttonResetRotation.Location = new System.Drawing.Point(13, 143);
            this.buttonResetRotation.Name = "buttonResetRotation";
            this.buttonResetRotation.Size = new System.Drawing.Size(109, 23);
            this.buttonResetRotation.TabIndex = 16;
            this.buttonResetRotation.Text = "Reset Rotation";
            this.buttonResetRotation.UseVisualStyleBackColor = true;
            this.buttonResetRotation.Click += new System.EventHandler(this.buttonResetRotation_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Rotation Pivot Point Location";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.trackPerspective);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonResetPerspective);
            this.groupBox2.Location = new System.Drawing.Point(12, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 103);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Perspective";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 56);
            this.label1.TabIndex = 17;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // BackgroundTransformatorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 383);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BackgroundTransformatorSettings";
            this.Text = "Background Transformation Settings";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.backgroundTransformatorSettingsFormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.trackHorizontalRotationOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRotationAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackVerticalRotationOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPerspective)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackHorizontalRotationOffset;
        private System.Windows.Forms.TrackBar trackRotationAngle;
        private System.Windows.Forms.TrackBar trackVerticalRotationOffset;
        private System.Windows.Forms.TrackBar trackPerspective;
        private System.Windows.Forms.Button buttonResetPerspective;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonResetRotation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkDrawPivotPoint;
        private System.Windows.Forms.Label label1;
    }
}