namespace mazio
{
    partial class Mazio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mazio));
            this.canvasHolderPanel = new System.Windows.Forms.Panel();
            this.labelSize = new System.Windows.Forms.Label();
            this.trackbarWidth = new System.Windows.Forms.TrackBar();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkImage = new System.Windows.Forms.CheckBox();
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnShowShapeSettings = new System.Windows.Forms.Button();
            this.btnCrop = new System.Windows.Forms.CheckBox();
            this.btnCensor = new System.Windows.Forms.CheckBox();
            this.btnMagnifier = new System.Windows.Forms.CheckBox();
            this.btnPonyVille = new System.Windows.Forms.CheckBox();
            this.btnTightCrop = new System.Windows.Forms.Button();
            this.btnCircle = new System.Windows.Forms.CheckBox();
            this.btnRectangle = new System.Windows.Forms.CheckBox();
            this.btnPencil = new System.Windows.Forms.CheckBox();
            this.btnText = new System.Windows.Forms.CheckBox();
            this.btnArrow = new System.Windows.Forms.CheckBox();
            this.btnLine = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.labelNewVersion = new System.Windows.Forms.LinkLabel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnCenter = new System.Windows.Forms.Button();
            this.btnFit = new System.Windows.Forms.Button();
            this.checkSaveAtFullSize = new System.Windows.Forms.CheckBox();
            this.btnZoomReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelZoom = new System.Windows.Forms.Label();
            this.trackbarZoom = new System.Windows.Forms.TrackBar();
            this.btnUpload = new System.Windows.Forms.Button();
            this.labelDrag = new System.Windows.Forms.Label();
            this.boxFileName = new System.Windows.Forms.TextBox();
            this.comboFileType = new System.Windows.Forms.ComboBox();
            this.comboUploadSite = new System.Windows.Forms.ComboBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnBottom = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnTop = new System.Windows.Forms.Button();
            this.labelOpacity = new System.Windows.Forms.Label();
            this.trackbarOpacity = new System.Windows.Forms.TrackBar();
            this.btnColor = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarWidth)).BeginInit();
            this.tablePanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarZoom)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarOpacity)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvasHolderPanel
            // 
            this.canvasHolderPanel.AutoScroll = true;
            this.canvasHolderPanel.AutoSize = true;
            this.canvasHolderPanel.BackColor = System.Drawing.Color.Tomato;
            this.canvasHolderPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.canvasHolderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasHolderPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.canvasHolderPanel.ForeColor = System.Drawing.Color.Red;
            this.canvasHolderPanel.Location = new System.Drawing.Point(0, 0);
            this.canvasHolderPanel.Name = "canvasHolderPanel";
            this.canvasHolderPanel.Size = new System.Drawing.Size(860, 557);
            this.canvasHolderPanel.TabIndex = 0;
            // 
            // labelSize
            // 
            this.labelSize.BackColor = System.Drawing.SystemColors.Control;
            this.labelSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSize.ForeColor = System.Drawing.Color.Black;
            this.labelSize.Location = new System.Drawing.Point(-3, 193);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(31, 26);
            this.labelSize.TabIndex = 3;
            this.labelSize.Text = "0";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSize.Paint += new System.Windows.Forms.PaintEventHandler(this.labelSize_Paint);
            // 
            // trackbarWidth
            // 
            this.trackbarWidth.Location = new System.Drawing.Point(-1, 127);
            this.trackbarWidth.Minimum = 1;
            this.trackbarWidth.Name = "trackbarWidth";
            this.trackbarWidth.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackbarWidth.Size = new System.Drawing.Size(45, 63);
            this.trackbarWidth.TabIndex = 2;
            this.trackbarWidth.TabStop = false;
            this.trackbarWidth.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarWidth.Value = 1;
            this.trackbarWidth.ValueChanged += new System.EventHandler(this.trackbarWidth_ValueChanged);
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Mazio";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            // 
            // tablePanel
            // 
            this.tablePanel.ColumnCount = 3;
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tablePanel.Controls.Add(this.panel2, 0, 1);
            this.tablePanel.Controls.Add(this.panel3, 0, 0);
            this.tablePanel.Controls.Add(this.panel4, 0, 2);
            this.tablePanel.Controls.Add(this.panel5, 2, 1);
            this.tablePanel.Controls.Add(this.panel1, 1, 1);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 3;
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanel.Size = new System.Drawing.Size(934, 631);
            this.tablePanel.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkImage);
            this.panel2.Controls.Add(this.btnTransform);
            this.panel2.Controls.Add(this.btnShowShapeSettings);
            this.panel2.Controls.Add(this.btnCrop);
            this.panel2.Controls.Add(this.btnCensor);
            this.panel2.Controls.Add(this.btnMagnifier);
            this.panel2.Controls.Add(this.btnPonyVille);
            this.panel2.Controls.Add(this.btnTightCrop);
            this.panel2.Controls.Add(this.btnCircle);
            this.panel2.Controls.Add(this.btnRectangle);
            this.panel2.Controls.Add(this.btnPencil);
            this.panel2.Controls.Add(this.btnText);
            this.panel2.Controls.Add(this.btnArrow);
            this.panel2.Controls.Add(this.btnLine);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(32, 567);
            this.panel2.TabIndex = 1;
            // 
            // checkImage
            // 
            this.checkImage.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkImage.FlatAppearance.BorderSize = 0;
            this.checkImage.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.checkImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkImage.Image = global::mazio.Properties.Resources.picturebox;
            this.checkImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkImage.Location = new System.Drawing.Point(5, 267);
            this.checkImage.Name = "checkImage";
            this.checkImage.Size = new System.Drawing.Size(26, 23);
            this.checkImage.TabIndex = 16;
            this.checkImage.TabStop = false;
            this.checkImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkImage.UseVisualStyleBackColor = true;
            this.checkImage.Click += new System.EventHandler(this.checkImage_Click);
            // 
            // btnTransform
            // 
            this.btnTransform.Enabled = false;
            this.btnTransform.FlatAppearance.BorderSize = 0;
            this.btnTransform.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnTransform.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransform.Image = global::mazio.Properties.Resources.shape_handles;
            this.btnTransform.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransform.Location = new System.Drawing.Point(6, 478);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(26, 23);
            this.btnTransform.TabIndex = 15;
            this.btnTransform.TabStop = false;
            this.btnTransform.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Visible = false;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // btnShowShapeSettings
            // 
            this.btnShowShapeSettings.FlatAppearance.BorderSize = 0;
            this.btnShowShapeSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowShapeSettings.Image = global::mazio.Properties.Resources.emblem_system;
            this.btnShowShapeSettings.Location = new System.Drawing.Point(6, 330);
            this.btnShowShapeSettings.Name = "btnShowShapeSettings";
            this.btnShowShapeSettings.Size = new System.Drawing.Size(26, 23);
            this.btnShowShapeSettings.TabIndex = 14;
            this.btnShowShapeSettings.TabStop = false;
            this.btnShowShapeSettings.UseVisualStyleBackColor = true;
            this.btnShowShapeSettings.Click += new System.EventHandler(this.buttonShowShapeSettings_Click);
            // 
            // btnCrop
            // 
            this.btnCrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCrop.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCrop.FlatAppearance.BorderSize = 0;
            this.btnCrop.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrop.Image = global::mazio.Properties.Resources.stock_tool_crop_16;
            this.btnCrop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCrop.Location = new System.Drawing.Point(6, 515);
            this.btnCrop.Name = "btnCrop";
            this.btnCrop.Size = new System.Drawing.Size(26, 23);
            this.btnCrop.TabIndex = 13;
            this.btnCrop.TabStop = false;
            this.btnCrop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCrop.UseVisualStyleBackColor = true;
            this.btnCrop.Click += new System.EventHandler(this.buttonCrop_Click);
            // 
            // btnCensor
            // 
            this.btnCensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCensor.FlatAppearance.BorderSize = 0;
            this.btnCensor.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnCensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCensor.Image = global::mazio.Properties.Resources.censor;
            this.btnCensor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCensor.Location = new System.Drawing.Point(5, 209);
            this.btnCensor.Name = "btnCensor";
            this.btnCensor.Size = new System.Drawing.Size(26, 23);
            this.btnCensor.TabIndex = 11;
            this.btnCensor.TabStop = false;
            this.btnCensor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCensor.UseVisualStyleBackColor = true;
            this.btnCensor.Click += new System.EventHandler(this.buttonCensor_Click);
            // 
            // btnMagnifier
            // 
            this.btnMagnifier.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnMagnifier.FlatAppearance.BorderSize = 0;
            this.btnMagnifier.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnMagnifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMagnifier.Image = global::mazio.Properties.Resources.zoom;
            this.btnMagnifier.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMagnifier.Location = new System.Drawing.Point(5, 180);
            this.btnMagnifier.Name = "btnMagnifier";
            this.btnMagnifier.Size = new System.Drawing.Size(26, 23);
            this.btnMagnifier.TabIndex = 10;
            this.btnMagnifier.TabStop = false;
            this.btnMagnifier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMagnifier.UseVisualStyleBackColor = true;
            this.btnMagnifier.Click += new System.EventHandler(this.buttonMagnifier_Click);
            // 
            // btnPonyVille
            // 
            this.btnPonyVille.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPonyVille.FlatAppearance.BorderSize = 0;
            this.btnPonyVille.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnPonyVille.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPonyVille.Image = global::mazio.Properties.Resources.emblem_favorite_16;
            this.btnPonyVille.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPonyVille.Location = new System.Drawing.Point(5, 238);
            this.btnPonyVille.Name = "btnPonyVille";
            this.btnPonyVille.Size = new System.Drawing.Size(26, 23);
            this.btnPonyVille.TabIndex = 9;
            this.btnPonyVille.TabStop = false;
            this.btnPonyVille.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPonyVille.UseVisualStyleBackColor = true;
            this.btnPonyVille.Click += new System.EventHandler(this.buttonPonyVille_Click);
            // 
            // btnTightCrop
            // 
            this.btnTightCrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTightCrop.Enabled = false;
            this.btnTightCrop.FlatAppearance.BorderSize = 0;
            this.btnTightCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTightCrop.Image = global::mazio.Properties.Resources.arrow_in;
            this.btnTightCrop.Location = new System.Drawing.Point(6, 544);
            this.btnTightCrop.Name = "btnTightCrop";
            this.btnTightCrop.Size = new System.Drawing.Size(26, 23);
            this.btnTightCrop.TabIndex = 0;
            this.btnTightCrop.TabStop = false;
            this.btnTightCrop.UseVisualStyleBackColor = true;
            this.btnTightCrop.Click += new System.EventHandler(this.buttonTightCrop_Click);
            // 
            // btnCircle
            // 
            this.btnCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCircle.FlatAppearance.BorderSize = 0;
            this.btnCircle.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnCircle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCircle.Image = global::mazio.Properties.Resources.circle;
            this.btnCircle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCircle.Location = new System.Drawing.Point(5, 64);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(26, 23);
            this.btnCircle.TabIndex = 5;
            this.btnCircle.TabStop = false;
            this.btnCircle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCircle.UseVisualStyleBackColor = true;
            this.btnCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnRectangle.FlatAppearance.BorderSize = 0;
            this.btnRectangle.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnRectangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRectangle.Image = global::mazio.Properties.Resources.rectangle;
            this.btnRectangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRectangle.Location = new System.Drawing.Point(5, 93);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(26, 23);
            this.btnRectangle.TabIndex = 4;
            this.btnRectangle.TabStop = false;
            this.btnRectangle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRectangle.UseVisualStyleBackColor = true;
            this.btnRectangle.Click += new System.EventHandler(this.buttonRectangle_Click);
            // 
            // btnPencil
            // 
            this.btnPencil.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPencil.FlatAppearance.BorderSize = 0;
            this.btnPencil.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnPencil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPencil.Image = global::mazio.Properties.Resources.pencil;
            this.btnPencil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPencil.Location = new System.Drawing.Point(5, 122);
            this.btnPencil.Name = "btnPencil";
            this.btnPencil.Size = new System.Drawing.Size(26, 23);
            this.btnPencil.TabIndex = 3;
            this.btnPencil.TabStop = false;
            this.btnPencil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPencil.UseVisualStyleBackColor = true;
            this.btnPencil.Click += new System.EventHandler(this.buttonPencil_Click);
            // 
            // btnText
            // 
            this.btnText.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnText.FlatAppearance.BorderSize = 0;
            this.btnText.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnText.Image = global::mazio.Properties.Resources.text;
            this.btnText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnText.Location = new System.Drawing.Point(5, 151);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(26, 23);
            this.btnText.TabIndex = 2;
            this.btnText.TabStop = false;
            this.btnText.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Click += new System.EventHandler(this.buttonText_Click);
            // 
            // btnArrow
            // 
            this.btnArrow.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnArrow.FlatAppearance.BorderSize = 0;
            this.btnArrow.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnArrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArrow.Image = global::mazio.Properties.Resources.arrow;
            this.btnArrow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArrow.Location = new System.Drawing.Point(6, 6);
            this.btnArrow.Name = "btnArrow";
            this.btnArrow.Size = new System.Drawing.Size(26, 23);
            this.btnArrow.TabIndex = 1;
            this.btnArrow.TabStop = false;
            this.btnArrow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnArrow.UseVisualStyleBackColor = true;
            this.btnArrow.Click += new System.EventHandler(this.buttonArrow_Click);
            // 
            // btnLine
            // 
            this.btnLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnLine.FlatAppearance.BorderSize = 0;
            this.btnLine.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine.Image = global::mazio.Properties.Resources.line;
            this.btnLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLine.Location = new System.Drawing.Point(5, 35);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(26, 23);
            this.btnLine.TabIndex = 0;
            this.btnLine.TabStop = false;
            this.btnLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLine.UseVisualStyleBackColor = true;
            this.btnLine.Click += new System.EventHandler(this.buttonLine_Click);
            // 
            // panel3
            // 
            this.tablePanel.SetColumnSpan(this.panel3, 3);
            this.panel3.Controls.Add(this.btnPaste);
            this.panel3.Controls.Add(this.btnFile);
            this.panel3.Controls.Add(this.labelNewVersion);
            this.panel3.Controls.Add(this.btnSettings);
            this.panel3.Controls.Add(this.btnRedo);
            this.panel3.Controls.Add(this.btnAbout);
            this.panel3.Controls.Add(this.btnUndo);
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Controls.Add(this.btnGrab);
            this.panel3.Controls.Add(this.btnCopy);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(928, 26);
            this.panel3.TabIndex = 2;
            // 
            // btnPaste
            // 
            this.btnPaste.FlatAppearance.BorderSize = 0;
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.Image = global::mazio.Properties.Resources.edit_paste;
            this.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaste.Location = new System.Drawing.Point(500, 3);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(110, 23);
            this.btnPaste.TabIndex = 8;
            this.btnPaste.TabStop = false;
            this.btnPaste.Text = "Paste Image...";
            this.btnPaste.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.buttonPaste_Click);
            // 
            // btnFile
            // 
            this.btnFile.FlatAppearance.BorderSize = 0;
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFile.Image = global::mazio.Properties.Resources.document_open;
            this.btnFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFile.Location = new System.Drawing.Point(5, 3);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(70, 23);
            this.btnFile.TabIndex = 7;
            this.btnFile.TabStop = false;
            this.btnFile.Text = "File";
            this.btnFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // labelNewVersion
            // 
            this.labelNewVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNewVersion.Enabled = false;
            this.labelNewVersion.Image = global::mazio.Properties.Resources.update;
            this.labelNewVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelNewVersion.Location = new System.Drawing.Point(719, 8);
            this.labelNewVersion.Name = "labelNewVersion";
            this.labelNewVersion.Size = new System.Drawing.Size(132, 13);
            this.labelNewVersion.TabIndex = 6;
            this.labelNewVersion.TabStop = true;
            this.labelNewVersion.Text = "New version available";
            this.labelNewVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelNewVersion.Visible = false;
            this.labelNewVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelNewVersion_LinkClicked);
            // 
            // btnSettings
            // 
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Image = global::mazio.Properties.Resources.preferences_system;
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.Location = new System.Drawing.Point(624, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(70, 23);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.TabStop = false;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Enabled = false;
            this.btnRedo.FlatAppearance.BorderSize = 0;
            this.btnRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedo.Image = global::mazio.Properties.Resources.edit_redo;
            this.btnRedo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedo.Location = new System.Drawing.Point(233, 3);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(70, 23);
            this.btnRedo.TabIndex = 4;
            this.btnRedo.TabStop = false;
            this.btnRedo.Text = "Redo";
            this.btnRedo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            this.btnRedo.MouseEnter += new System.EventHandler(this.buttonRedo_MouseEnter);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.FlatAppearance.BorderSize = 0;
            this.btnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbout.Image = global::mazio.Properties.Resources.help;
            this.btnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbout.Location = new System.Drawing.Point(857, 3);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(70, 23);
            this.btnAbout.TabIndex = 1;
            this.btnAbout.TabStop = false;
            this.btnAbout.Text = "About";
            this.btnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Enabled = false;
            this.btnUndo.FlatAppearance.BorderSize = 0;
            this.btnUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndo.Image = global::mazio.Properties.Resources.edit_undo;
            this.btnUndo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndo.Location = new System.Drawing.Point(157, 3);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(70, 23);
            this.btnUndo.TabIndex = 3;
            this.btnUndo.TabStop = false;
            this.btnUndo.Text = "Undo";
            this.btnUndo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            this.btnUndo.MouseEnter += new System.EventHandler(this.buttonUndo_MouseEnter);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Image = global::mazio.Properties.Resources.edit_clear;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.Location = new System.Drawing.Point(309, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // btnGrab
            // 
            this.btnGrab.FlatAppearance.BorderSize = 0;
            this.btnGrab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrab.Image = global::mazio.Properties.Resources.camera;
            this.btnGrab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrab.Location = new System.Drawing.Point(81, 3);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(70, 23);
            this.btnGrab.TabIndex = 1;
            this.btnGrab.TabStop = false;
            this.btnGrab.Text = "&Grab";
            this.btnGrab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.buttonGrab_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Image = global::mazio.Properties.Resources.edit_copy;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopy.Location = new System.Drawing.Point(400, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(94, 23);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.TabStop = false;
            this.btnCopy.Text = "Copy Image";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // panel4
            // 
            this.tablePanel.SetColumnSpan(this.panel4, 2);
            this.panel4.Controls.Add(this.btnCenter);
            this.panel4.Controls.Add(this.btnFit);
            this.panel4.Controls.Add(this.checkSaveAtFullSize);
            this.panel4.Controls.Add(this.btnZoomReset);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.labelZoom);
            this.panel4.Controls.Add(this.trackbarZoom);
            this.panel4.Controls.Add(this.btnUpload);
            this.panel4.Controls.Add(this.labelDrag);
            this.panel4.Controls.Add(this.boxFileName);
            this.panel4.Controls.Add(this.comboFileType);
            this.panel4.Controls.Add(this.comboUploadSite);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 602);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(896, 26);
            this.panel4.TabIndex = 3;
            // 
            // btnCenter
            // 
            this.btnCenter.Enabled = false;
            this.btnCenter.FlatAppearance.BorderSize = 0;
            this.btnCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenter.Location = new System.Drawing.Point(282, 2);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Size = new System.Drawing.Size(52, 23);
            this.btnCenter.TabIndex = 18;
            this.btnCenter.TabStop = false;
            this.btnCenter.Text = "Center";
            this.btnCenter.UseVisualStyleBackColor = true;
            this.btnCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // btnFit
            // 
            this.btnFit.Enabled = false;
            this.btnFit.FlatAppearance.BorderSize = 0;
            this.btnFit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFit.Location = new System.Drawing.Point(227, 2);
            this.btnFit.Name = "btnFit";
            this.btnFit.Size = new System.Drawing.Size(49, 23);
            this.btnFit.TabIndex = 17;
            this.btnFit.TabStop = false;
            this.btnFit.Text = "Fit";
            this.btnFit.UseVisualStyleBackColor = true;
            this.btnFit.Click += new System.EventHandler(this.buttonFit_Click);
            // 
            // checkSaveAtFullSize
            // 
            this.checkSaveAtFullSize.AutoSize = true;
            this.checkSaveAtFullSize.Checked = true;
            this.checkSaveAtFullSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSaveAtFullSize.Enabled = false;
            this.checkSaveAtFullSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkSaveAtFullSize.Location = new System.Drawing.Point(353, 5);
            this.checkSaveAtFullSize.Name = "checkSaveAtFullSize";
            this.checkSaveAtFullSize.Size = new System.Drawing.Size(102, 17);
            this.checkSaveAtFullSize.TabIndex = 16;
            this.checkSaveAtFullSize.Text = "Save at Full Size";
            this.checkSaveAtFullSize.UseVisualStyleBackColor = true;
            // 
            // btnZoomReset
            // 
            this.btnZoomReset.Enabled = false;
            this.btnZoomReset.FlatAppearance.BorderSize = 0;
            this.btnZoomReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomReset.Location = new System.Drawing.Point(186, 2);
            this.btnZoomReset.Name = "btnZoomReset";
            this.btnZoomReset.Size = new System.Drawing.Size(41, 23);
            this.btnZoomReset.TabIndex = 12;
            this.btnZoomReset.TabStop = false;
            this.btnZoomReset.Text = "1:1";
            this.btnZoomReset.UseVisualStyleBackColor = true;
            this.btnZoomReset.Click += new System.EventHandler(this.buttonZoomReset_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Zoom:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelZoom
            // 
            this.labelZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelZoom.AutoSize = true;
            this.labelZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelZoom.ForeColor = System.Drawing.Color.Black;
            this.labelZoom.Location = new System.Drawing.Point(141, 7);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(33, 13);
            this.labelZoom.TabIndex = 14;
            this.labelZoom.Text = "100%";
            this.labelZoom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trackbarZoom
            // 
            this.trackbarZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackbarZoom.Enabled = false;
            this.trackbarZoom.Location = new System.Drawing.Point(72, 2);
            this.trackbarZoom.Maximum = 200;
            this.trackbarZoom.Minimum = 20;
            this.trackbarZoom.Name = "trackbarZoom";
            this.trackbarZoom.Size = new System.Drawing.Size(63, 45);
            this.trackbarZoom.SmallChange = 20;
            this.trackbarZoom.TabIndex = 14;
            this.trackbarZoom.TabStop = false;
            this.trackbarZoom.TickFrequency = 20;
            this.trackbarZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarZoom.Value = 100;
            this.trackbarZoom.ValueChanged += new System.EventHandler(this.trackbarZoom_ValueChanged);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Location = new System.Drawing.Point(712, 2);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.TabStop = false;
            this.btnUpload.Text = "Upload to ->";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // labelDrag
            // 
            this.labelDrag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDrag.AutoSize = true;
            this.labelDrag.BackColor = System.Drawing.Color.Red;
            this.labelDrag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDrag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDrag.ForeColor = System.Drawing.Color.White;
            this.labelDrag.Location = new System.Drawing.Point(627, 2);
            this.labelDrag.Name = "labelDrag";
            this.labelDrag.Size = new System.Drawing.Size(79, 22);
            this.labelDrag.TabIndex = 5;
            this.labelDrag.Text = "Drag Me";
            this.labelDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelDrag_MouseDown);
            // 
            // boxFileName
            // 
            this.boxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.boxFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxFileName.Location = new System.Drawing.Point(466, 3);
            this.boxFileName.Name = "boxFileName";
            this.boxFileName.Size = new System.Drawing.Size(104, 20);
            this.boxFileName.TabIndex = 3;
            this.boxFileName.TabStop = false;
            this.boxFileName.TextChanged += new System.EventHandler(this.fileName_TextChanged);
            // 
            // comboFileType
            // 
            this.comboFileType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFileType.BackColor = System.Drawing.SystemColors.Control;
            this.comboFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboFileType.FormattingEnabled = true;
            this.comboFileType.Location = new System.Drawing.Point(576, 2);
            this.comboFileType.Name = "comboFileType";
            this.comboFileType.Size = new System.Drawing.Size(45, 21);
            this.comboFileType.TabIndex = 1;
            this.comboFileType.TabStop = false;
            this.comboFileType.SelectedIndexChanged += new System.EventHandler(this.comboFileType_SelectedIndexChanged);
            // 
            // comboUploadSite
            // 
            this.comboUploadSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboUploadSite.BackColor = System.Drawing.SystemColors.Control;
            this.comboUploadSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUploadSite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboUploadSite.FormattingEnabled = true;
            this.comboUploadSite.Location = new System.Drawing.Point(793, 3);
            this.comboUploadSite.Name = "comboUploadSite";
            this.comboUploadSite.Size = new System.Drawing.Size(100, 21);
            this.comboUploadSite.TabIndex = 0;
            this.comboUploadSite.TabStop = false;
            this.comboUploadSite.SelectedIndexChanged += new System.EventHandler(this.comboUploadSite_SelectedIndexChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnBottom);
            this.panel5.Controls.Add(this.btnDown);
            this.panel5.Controls.Add(this.btnUp);
            this.panel5.Controls.Add(this.btnTop);
            this.panel5.Controls.Add(this.labelOpacity);
            this.panel5.Controls.Add(this.trackbarOpacity);
            this.panel5.Controls.Add(this.labelSize);
            this.panel5.Controls.Add(this.btnColor);
            this.panel5.Controls.Add(this.trackbarWidth);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(905, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(26, 561);
            this.panel5.TabIndex = 4;
            // 
            // btnBottom
            // 
            this.btnBottom.Enabled = false;
            this.btnBottom.FlatAppearance.BorderSize = 0;
            this.btnBottom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBottom.Image = global::mazio.Properties.Resources.go_bottom;
            this.btnBottom.Location = new System.Drawing.Point(-1, 327);
            this.btnBottom.Name = "btnBottom";
            this.btnBottom.Size = new System.Drawing.Size(26, 23);
            this.btnBottom.TabIndex = 13;
            this.btnBottom.TabStop = false;
            this.btnBottom.UseVisualStyleBackColor = true;
            this.btnBottom.Click += new System.EventHandler(this.buttonBottom_Click);
            // 
            // btnDown
            // 
            this.btnDown.Enabled = false;
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Image = global::mazio.Properties.Resources.go_down;
            this.btnDown.Location = new System.Drawing.Point(-1, 298);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(26, 23);
            this.btnDown.TabIndex = 12;
            this.btnDown.TabStop = false;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Enabled = false;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Image = global::mazio.Properties.Resources.go_up;
            this.btnUp.Location = new System.Drawing.Point(-1, 269);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(26, 23);
            this.btnUp.TabIndex = 10;
            this.btnUp.TabStop = false;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // btnTop
            // 
            this.btnTop.Enabled = false;
            this.btnTop.FlatAppearance.BorderSize = 0;
            this.btnTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTop.Image = global::mazio.Properties.Resources.go_top;
            this.btnTop.Location = new System.Drawing.Point(-1, 240);
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(26, 23);
            this.btnTop.TabIndex = 11;
            this.btnTop.TabStop = false;
            this.btnTop.UseVisualStyleBackColor = true;
            this.btnTop.Click += new System.EventHandler(this.buttonTop_Click);
            // 
            // labelOpacity
            // 
            this.labelOpacity.AutoSize = true;
            this.labelOpacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelOpacity.ForeColor = System.Drawing.Color.Black;
            this.labelOpacity.Location = new System.Drawing.Point(-4, 99);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(33, 13);
            this.labelOpacity.TabIndex = 5;
            this.labelOpacity.Text = "100%";
            this.labelOpacity.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trackbarOpacity
            // 
            this.trackbarOpacity.Location = new System.Drawing.Point(-1, 32);
            this.trackbarOpacity.Minimum = 1;
            this.trackbarOpacity.Name = "trackbarOpacity";
            this.trackbarOpacity.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackbarOpacity.Size = new System.Drawing.Size(45, 63);
            this.trackbarOpacity.TabIndex = 4;
            this.trackbarOpacity.TabStop = false;
            this.trackbarOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarOpacity.Value = 1;
            this.trackbarOpacity.ValueChanged += new System.EventHandler(this.trackbarOpacity_ValueChanged);
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Red;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Location = new System.Drawing.Point(0, 3);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(25, 23);
            this.btnColor.TabIndex = 2;
            this.btnColor.TabStop = false;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.canvasHolderPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(35, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 561);
            this.panel1.TabIndex = 5;
            // 
            // Mazio
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(934, 631);
            this.Controls.Add(this.tablePanel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(950, 540);
            this.Name = "Mazio";
            this.Text = "Mazio";
            this.Load += new System.EventHandler(this.mazioLoad);
            this.VisibleChanged += new System.EventHandler(this.visibleChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropFile);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.dragEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.onKeyUp);
            this.Move += new System.EventHandler(this.onMove);
            this.Resize += new System.EventHandler(this.onResize);
            ((System.ComponentModel.ISupportInitialize)(this.trackbarWidth)).EndInit();
            this.tablePanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarZoom)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarOpacity)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Panel canvasHolderPanel;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.TrackBar trackbarWidth;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.ComboBox comboUploadSite;
        private System.Windows.Forms.Label labelDrag;
        private System.Windows.Forms.TextBox boxFileName;
        private System.Windows.Forms.ComboBox comboFileType;
        private System.Windows.Forms.CheckBox btnCircle;
        private System.Windows.Forms.CheckBox btnRectangle;
        private System.Windows.Forms.CheckBox btnPencil;
        private System.Windows.Forms.CheckBox btnText;
        private System.Windows.Forms.CheckBox btnArrow;
        private System.Windows.Forms.CheckBox btnLine;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Button btnTightCrop;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.LinkLabel labelNewVersion;
        private System.Windows.Forms.Label labelOpacity;
        private System.Windows.Forms.TrackBar trackbarOpacity;
        private System.Windows.Forms.CheckBox btnPonyVille;
        private System.Windows.Forms.Button btnBottom;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnTop;
        private System.Windows.Forms.CheckBox btnMagnifier;
        private System.Windows.Forms.CheckBox btnCensor;
        private System.Windows.Forms.TrackBar trackbarZoom;
        private System.Windows.Forms.Label labelZoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnZoomReset;
        private System.Windows.Forms.CheckBox checkSaveAtFullSize;
        private System.Windows.Forms.Button btnFit;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.CheckBox btnCrop;
        private System.Windows.Forms.Button btnCenter;
        private System.Windows.Forms.Button btnShowShapeSettings;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.CheckBox checkImage;
    }
}
