namespace mazio {
    partial class CropSettingsPanel {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.checkConstrainProportions = new System.Windows.Forms.CheckBox();
            this.groupProportions = new System.Windows.Forms.GroupBox();
            this.radioSquare = new System.Windows.Forms.RadioButton();
            this.numericH = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericW = new System.Windows.Forms.NumericUpDown();
            this.radioCustom = new System.Windows.Forms.RadioButton();
            this.radio16x9 = new System.Windows.Forms.RadioButton();
            this.radio4x3 = new System.Windows.Forms.RadioButton();
            this.groupProportions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericW)).BeginInit();
            this.SuspendLayout();
            // 
            // checkConstrainProportions
            // 
            this.checkConstrainProportions.AutoSize = true;
            this.checkConstrainProportions.Location = new System.Drawing.Point(4, 4);
            this.checkConstrainProportions.Name = "checkConstrainProportions";
            this.checkConstrainProportions.Size = new System.Drawing.Size(126, 17);
            this.checkConstrainProportions.TabIndex = 0;
            this.checkConstrainProportions.Text = "Constrain Proportions";
            this.checkConstrainProportions.UseVisualStyleBackColor = true;
            this.checkConstrainProportions.CheckedChanged += new System.EventHandler(this.checkConstrainProportions_CheckedChanged);
            // 
            // groupProportions
            // 
            this.groupProportions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProportions.Controls.Add(this.radioSquare);
            this.groupProportions.Controls.Add(this.numericH);
            this.groupProportions.Controls.Add(this.label1);
            this.groupProportions.Controls.Add(this.numericW);
            this.groupProportions.Controls.Add(this.radioCustom);
            this.groupProportions.Controls.Add(this.radio16x9);
            this.groupProportions.Controls.Add(this.radio4x3);
            this.groupProportions.Location = new System.Drawing.Point(4, 28);
            this.groupProportions.Name = "groupProportions";
            this.groupProportions.Size = new System.Drawing.Size(143, 140);
            this.groupProportions.TabIndex = 1;
            this.groupProportions.TabStop = false;
            this.groupProportions.Text = "Proportions";
            // 
            // radioSquare
            // 
            this.radioSquare.AutoSize = true;
            this.radioSquare.Location = new System.Drawing.Point(8, 19);
            this.radioSquare.Name = "radioSquare";
            this.radioSquare.Size = new System.Drawing.Size(59, 17);
            this.radioSquare.TabIndex = 6;
            this.radioSquare.TabStop = true;
            this.radioSquare.Text = "Square";
            this.radioSquare.UseVisualStyleBackColor = true;
            this.radioSquare.CheckedChanged += new System.EventHandler(this.radioSquare_CheckedChanged);
            // 
            // numericH
            // 
            this.numericH.Location = new System.Drawing.Point(92, 111);
            this.numericH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericH.Name = "numericH";
            this.numericH.Size = new System.Drawing.Size(42, 20);
            this.numericH.TabIndex = 5;
            this.numericH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericH.ValueChanged += new System.EventHandler(this.numericH_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "by";
            // 
            // numericW
            // 
            this.numericW.Location = new System.Drawing.Point(23, 111);
            this.numericW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericW.Name = "numericW";
            this.numericW.Size = new System.Drawing.Size(42, 20);
            this.numericW.TabIndex = 3;
            this.numericW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericW.ValueChanged += new System.EventHandler(this.numericW_ValueChanged);
            // 
            // radioCustom
            // 
            this.radioCustom.AutoSize = true;
            this.radioCustom.Location = new System.Drawing.Point(8, 88);
            this.radioCustom.Name = "radioCustom";
            this.radioCustom.Size = new System.Drawing.Size(60, 17);
            this.radioCustom.TabIndex = 2;
            this.radioCustom.TabStop = true;
            this.radioCustom.Text = "Custom";
            this.radioCustom.UseVisualStyleBackColor = true;
            this.radioCustom.CheckedChanged += new System.EventHandler(this.radioCustom_CheckedChanged);
            // 
            // radio16x9
            // 
            this.radio16x9.AutoSize = true;
            this.radio16x9.Location = new System.Drawing.Point(8, 65);
            this.radio16x9.Name = "radio16x9";
            this.radio16x9.Size = new System.Drawing.Size(54, 17);
            this.radio16x9.TabIndex = 1;
            this.radio16x9.TabStop = true;
            this.radio16x9.Text = "16 x 9";
            this.radio16x9.UseVisualStyleBackColor = true;
            this.radio16x9.CheckedChanged += new System.EventHandler(this.radio16X9CheckedChanged);
            // 
            // radio4x3
            // 
            this.radio4x3.AutoSize = true;
            this.radio4x3.Location = new System.Drawing.Point(8, 42);
            this.radio4x3.Name = "radio4x3";
            this.radio4x3.Size = new System.Drawing.Size(48, 17);
            this.radio4x3.TabIndex = 0;
            this.radio4x3.TabStop = true;
            this.radio4x3.Text = "4 x 3";
            this.radio4x3.UseVisualStyleBackColor = true;
            this.radio4x3.CheckedChanged += new System.EventHandler(this.radio4X3CheckedChanged);
            // 
            // CropSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupProportions);
            this.Controls.Add(this.checkConstrainProportions);
            this.Name = "CropSettingsPanel";
            this.Size = new System.Drawing.Size(150, 171);
            this.groupProportions.ResumeLayout(false);
            this.groupProportions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericW)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkConstrainProportions;
        private System.Windows.Forms.GroupBox groupProportions;
        private System.Windows.Forms.NumericUpDown numericH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericW;
        private System.Windows.Forms.RadioButton radioCustom;
        private System.Windows.Forms.RadioButton radio16x9;
        private System.Windows.Forms.RadioButton radio4x3;
        private System.Windows.Forms.RadioButton radioSquare;
    }
}
