namespace FaceDetection
{
    partial class MainForm
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
			this.WebCamCapture = new WebCam_Capture.WebCamCapture();
			this.fixedPicture = new System.Windows.Forms.PictureBox();
			this.analize_btn = new System.Windows.Forms.Button();
			this.webCamPicture = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.sideLongTiltLabel = new System.Windows.Forms.Label();
			this.dexpected = new System.Windows.Forms.Label();
			this.deyes = new System.Windows.Forms.Label();
			this.Dkoef = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.rotationAroundVerticalOxLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.backForthAngleLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.fiF = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ParentPanel = new System.Windows.Forms.Panel();
			this.photoPathTextBox = new System.Windows.Forms.TextBox();
			this.startCalibration_btn = new System.Windows.Forms.Button();
			this.saveCalibration_btn = new System.Windows.Forms.Button();
			this.getCalibrationPoint_btn = new System.Windows.Forms.Button();
			this.backgroundPanel = new System.Windows.Forms.Panel();
			this.SAngleLabel = new System.Windows.Forms.Label();
			this.ShowPanels_btn = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.HidePanels_btn = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).BeginInit();
			this.panel1.SuspendLayout();
			this.ParentPanel.SuspendLayout();
			this.backgroundPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// WebCamCapture
			// 
			this.WebCamCapture.CaptureHeight = 240;
			this.WebCamCapture.CaptureWidth = 320;
			this.WebCamCapture.FrameNumber = ((ulong)(0ul));
			this.WebCamCapture.Location = new System.Drawing.Point(17, 17);
			this.WebCamCapture.Name = "WebCamCapture";
			this.WebCamCapture.Size = new System.Drawing.Size(342, 252);
			this.WebCamCapture.TabIndex = 0;
			this.WebCamCapture.TimeToCapture_milliseconds = 100;
			this.WebCamCapture.ImageCaptured += new WebCam_Capture.WebCamCapture.WebCamEventHandler(this.WebCamCapture_ImageCaptured);
			// 
			// fixedPicture
			// 
			this.fixedPicture.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.fixedPicture.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.fixedPicture.Location = new System.Drawing.Point(649, -47);
			this.fixedPicture.Name = "fixedPicture";
			this.fixedPicture.Size = new System.Drawing.Size(448, 381);
			this.fixedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.fixedPicture.TabIndex = 4;
			this.fixedPicture.TabStop = false;
			// 
			// analize_btn
			// 
			this.analize_btn.Location = new System.Drawing.Point(334, 540);
			this.analize_btn.Name = "analize_btn";
			this.analize_btn.Size = new System.Drawing.Size(96, 38);
			this.analize_btn.TabIndex = 37;
			this.analize_btn.Text = "Analize";
			this.analize_btn.UseVisualStyleBackColor = true;
			this.analize_btn.Click += new System.EventHandler(this.Analize_btn_Click);
			// 
			// webCamPicture
			// 
			this.webCamPicture.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.webCamPicture.Location = new System.Drawing.Point(3, 0);
			this.webCamPicture.Name = "webCamPicture";
			this.webCamPicture.Size = new System.Drawing.Size(640, 480);
			this.webCamPicture.TabIndex = 15;
			this.webCamPicture.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Боковой наклон";
			// 
			// sideLongTiltLabel
			// 
			this.sideLongTiltLabel.AutoSize = true;
			this.sideLongTiltLabel.Location = new System.Drawing.Point(108, 29);
			this.sideLongTiltLabel.Name = "sideLongTiltLabel";
			this.sideLongTiltLabel.Size = new System.Drawing.Size(0, 13);
			this.sideLongTiltLabel.TabIndex = 17;
			// 
			// dexpected
			// 
			this.dexpected.AutoSize = true;
			this.dexpected.Location = new System.Drawing.Point(296, 571);
			this.dexpected.Name = "dexpected";
			this.dexpected.Size = new System.Drawing.Size(0, 13);
			this.dexpected.TabIndex = 20;
			// 
			// deyes
			// 
			this.deyes.AutoSize = true;
			this.deyes.Location = new System.Drawing.Point(428, 571);
			this.deyes.Name = "deyes";
			this.deyes.Size = new System.Drawing.Size(0, 13);
			this.deyes.TabIndex = 21;
			// 
			// Dkoef
			// 
			this.Dkoef.AutoSize = true;
			this.Dkoef.Location = new System.Drawing.Point(653, 571);
			this.Dkoef.Name = "Dkoef";
			this.Dkoef.Size = new System.Drawing.Size(0, 13);
			this.Dkoef.TabIndex = 23;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(244, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 13);
			this.label2.TabIndex = 30;
			this.label2.Text = "Вокруг вертикальной оси";
			// 
			// rotationAroundVerticalOxLabel
			// 
			this.rotationAroundVerticalOxLabel.AutoSize = true;
			this.rotationAroundVerticalOxLabel.Location = new System.Drawing.Point(398, 29);
			this.rotationAroundVerticalOxLabel.Name = "rotationAroundVerticalOxLabel";
			this.rotationAroundVerticalOxLabel.Size = new System.Drawing.Size(0, 13);
			this.rotationAroundVerticalOxLabel.TabIndex = 31;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(626, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "ВпередНазад";
			// 
			// backForthAngleLabel
			// 
			this.backForthAngleLabel.AutoSize = true;
			this.backForthAngleLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.backForthAngleLabel.Location = new System.Drawing.Point(725, 29);
			this.backForthAngleLabel.Name = "backForthAngleLabel";
			this.backForthAngleLabel.Size = new System.Drawing.Size(0, 13);
			this.backForthAngleLabel.TabIndex = 33;
			this.backForthAngleLabel.UseMnemonic = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.fiF);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.rotationAroundVerticalOxLabel);
			this.panel1.Controls.Add(this.backForthAngleLabel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.sideLongTiltLabel);
			this.panel1.Location = new System.Drawing.Point(3, 482);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(910, 55);
			this.panel1.TabIndex = 34;
			// 
			// fiF
			// 
			this.fiF.AutoSize = true;
			this.fiF.Location = new System.Drawing.Point(84, 13);
			this.fiF.Name = "fiF";
			this.fiF.Size = new System.Drawing.Size(0, 13);
			this.fiF.TabIndex = 35;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 13);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 13);
			this.label4.TabIndex = 34;
			this.label4.Text = "fiFrameAngle";
			// 
			// ParentPanel
			// 
			this.ParentPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ParentPanel.Controls.Add(this.photoPathTextBox);
			this.ParentPanel.Controls.Add(this.startCalibration_btn);
			this.ParentPanel.Controls.Add(this.analize_btn);
			this.ParentPanel.Controls.Add(this.saveCalibration_btn);
			this.ParentPanel.Controls.Add(this.panel1);
			this.ParentPanel.Controls.Add(this.fixedPicture);
			this.ParentPanel.Controls.Add(this.webCamPicture);
			this.ParentPanel.Location = new System.Drawing.Point(135, 24);
			this.ParentPanel.MinimumSize = new System.Drawing.Size(400, 400);
			this.ParentPanel.Name = "ParentPanel";
			this.ParentPanel.Size = new System.Drawing.Size(1108, 584);
			this.ParentPanel.TabIndex = 35;
			// 
			// photoPathTextBox
			// 
			this.photoPathTextBox.Location = new System.Drawing.Point(210, 540);
			this.photoPathTextBox.Name = "photoPathTextBox";
			this.photoPathTextBox.Size = new System.Drawing.Size(100, 20);
			this.photoPathTextBox.TabIndex = 38;
			this.photoPathTextBox.Text = "Images/Image1.jpg";
			// 
			// startCalibration_btn
			// 
			this.startCalibration_btn.Location = new System.Drawing.Point(3, 540);
			this.startCalibration_btn.Name = "startCalibration_btn";
			this.startCalibration_btn.Size = new System.Drawing.Size(90, 29);
			this.startCalibration_btn.TabIndex = 38;
			this.startCalibration_btn.Text = "start calibration";
			this.startCalibration_btn.UseVisualStyleBackColor = true;
			this.startCalibration_btn.Click += new System.EventHandler(this.StartCalibration_btn_Click);
			// 
			// saveCalibration_btn
			// 
			this.saveCalibration_btn.Location = new System.Drawing.Point(101, 540);
			this.saveCalibration_btn.Name = "saveCalibration_btn";
			this.saveCalibration_btn.Size = new System.Drawing.Size(87, 30);
			this.saveCalibration_btn.TabIndex = 39;
			this.saveCalibration_btn.Text = "saveCalibration";
			this.saveCalibration_btn.UseVisualStyleBackColor = true;
			this.saveCalibration_btn.Click += new System.EventHandler(this.SaveCalibration_Click);
			// 
			// getCalibrationPoint_btn
			// 
			this.getCalibrationPoint_btn.Location = new System.Drawing.Point(1157, 608);
			this.getCalibrationPoint_btn.Name = "getCalibrationPoint_btn";
			this.getCalibrationPoint_btn.Size = new System.Drawing.Size(86, 37);
			this.getCalibrationPoint_btn.TabIndex = 36;
			this.getCalibrationPoint_btn.Text = "GetCalibrationPoint";
			this.getCalibrationPoint_btn.UseVisualStyleBackColor = true;
			this.getCalibrationPoint_btn.Visible = false;
			this.getCalibrationPoint_btn.Click += new System.EventHandler(this.Calibrate_btn_Click);
			// 
			// backgroundPanel
			// 
			this.backgroundPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.backgroundPanel.Controls.Add(this.HidePanels_btn);
			this.backgroundPanel.Controls.Add(this.SAngleLabel);
			this.backgroundPanel.Controls.Add(this.ShowPanels_btn);
			this.backgroundPanel.Controls.Add(this.label5);
			this.backgroundPanel.Controls.Add(this.ParentPanel);
			this.backgroundPanel.Controls.Add(this.getCalibrationPoint_btn);
			this.backgroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.backgroundPanel.Location = new System.Drawing.Point(0, 0);
			this.backgroundPanel.Name = "backgroundPanel";
			this.backgroundPanel.Size = new System.Drawing.Size(1298, 742);
			this.backgroundPanel.TabIndex = 40;
			// 
			// SAngleLabel
			// 
			this.SAngleLabel.AutoSize = true;
			this.SAngleLabel.Location = new System.Drawing.Point(80, 632);
			this.SAngleLabel.Name = "SAngleLabel";
			this.SAngleLabel.Size = new System.Drawing.Size(0, 13);
			this.SAngleLabel.TabIndex = 2;
			// 
			// ShowPanels_btn
			// 
			this.ShowPanels_btn.Location = new System.Drawing.Point(26, 658);
			this.ShowPanels_btn.Name = "ShowPanels_btn";
			this.ShowPanels_btn.Size = new System.Drawing.Size(90, 37);
			this.ShowPanels_btn.TabIndex = 0;
			this.ShowPanels_btn.Text = "ShowPanels";
			this.ShowPanels_btn.UseVisualStyleBackColor = true;
			this.ShowPanels_btn.Visible = false;
			this.ShowPanels_btn.Click += new System.EventHandler(this.ShowPanels_btn_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(23, 632);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "SAngle";
			// 
			// HidePanels_btn
			// 
			this.HidePanels_btn.Location = new System.Drawing.Point(26, 658);
			this.HidePanels_btn.Name = "HidePanels_btn";
			this.HidePanels_btn.Size = new System.Drawing.Size(90, 37);
			this.HidePanels_btn.TabIndex = 37;
			this.HidePanels_btn.Text = "HidePanels";
			this.HidePanels_btn.UseVisualStyleBackColor = true;
			this.HidePanels_btn.Click += new System.EventHandler(this.HidePanels_btn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1298, 742);
			this.Controls.Add(this.Dkoef);
			this.Controls.Add(this.deyes);
			this.Controls.Add(this.dexpected);
			this.Controls.Add(this.backgroundPanel);
			this.Name = "MainForm";
			this.Text = "Eye finder";
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ParentPanel.ResumeLayout(false);
			this.ParentPanel.PerformLayout();
			this.backgroundPanel.ResumeLayout(false);
			this.backgroundPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.PictureBox fixedPicture;
		private System.Windows.Forms.PictureBox webCamPicture;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label sideLongTiltLabel;
		private System.Windows.Forms.Label dexpected;
		private System.Windows.Forms.Label deyes;
		private System.Windows.Forms.Label Dkoef;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label rotationAroundVerticalOxLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label backForthAngleLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel ParentPanel;
		private System.Windows.Forms.Button getCalibrationPoint_btn;
		private System.Windows.Forms.Button analize_btn;
		private System.Windows.Forms.Button startCalibration_btn;
		private System.Windows.Forms.Button saveCalibration_btn;
		private System.Windows.Forms.Label fiF;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel backgroundPanel;
		private System.Windows.Forms.TextBox photoPathTextBox;
		private System.Windows.Forms.Label SAngleLabel;
		private System.Windows.Forms.Button ShowPanels_btn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button HidePanels_btn;
    }
}

