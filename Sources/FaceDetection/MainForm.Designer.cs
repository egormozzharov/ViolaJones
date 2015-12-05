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
			this.button1 = new System.Windows.Forms.Button();
			this.fixedPicture = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.catchPicture_button = new System.Windows.Forms.Button();
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
			this.ParentPanel = new System.Windows.Forms.Panel();
			this.calibrate_btn = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).BeginInit();
			this.panel1.SuspendLayout();
			this.ParentPanel.SuspendLayout();
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
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(469, 18);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(431, 46);
			this.button1.TabIndex = 5;
			this.button1.Text = "Detect";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Detect_buttonClick);
			// 
			// fixedPicture
			// 
			this.fixedPicture.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.fixedPicture.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.fixedPicture.Location = new System.Drawing.Point(481, 10);
			this.fixedPicture.Name = "fixedPicture";
			this.fixedPicture.Size = new System.Drawing.Size(448, 381);
			this.fixedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.fixedPicture.TabIndex = 4;
			this.fixedPicture.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.catchPicture_button);
			this.panel2.Controls.Add(this.button1);
			this.panel2.Location = new System.Drawing.Point(18, 407);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(917, 82);
			this.panel2.TabIndex = 14;
			// 
			// catchPicture_button
			// 
			this.catchPicture_button.Location = new System.Drawing.Point(16, 19);
			this.catchPicture_button.Name = "catchPicture_button";
			this.catchPicture_button.Size = new System.Drawing.Size(431, 45);
			this.catchPicture_button.TabIndex = 1;
			this.catchPicture_button.Text = "Catch";
			this.catchPicture_button.UseVisualStyleBackColor = true;
			this.catchPicture_button.Click += new System.EventHandler(this.CatchPicture_button_Click);
			// 
			// webCamPicture
			// 
			this.webCamPicture.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.webCamPicture.Location = new System.Drawing.Point(15, 10);
			this.webCamPicture.Name = "webCamPicture";
			this.webCamPicture.Size = new System.Drawing.Size(447, 381);
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
			this.label2.Location = new System.Drawing.Point(247, 29);
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
			this.label3.Location = new System.Drawing.Point(623, 29);
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
			this.panel1.Controls.Add(this.rotationAroundVerticalOxLabel);
			this.panel1.Controls.Add(this.backForthAngleLabel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.sideLongTiltLabel);
			this.panel1.Location = new System.Drawing.Point(18, 501);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(910, 55);
			this.panel1.TabIndex = 34;
			// 
			// ParentPanel
			// 
			this.ParentPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ParentPanel.Controls.Add(this.panel1);
			this.ParentPanel.Controls.Add(this.fixedPicture);
			this.ParentPanel.Controls.Add(this.webCamPicture);
			this.ParentPanel.Controls.Add(this.panel2);
			this.ParentPanel.Location = new System.Drawing.Point(12, 12);
			this.ParentPanel.MinimumSize = new System.Drawing.Size(400, 400);
			this.ParentPanel.Name = "ParentPanel";
			this.ParentPanel.Size = new System.Drawing.Size(938, 572);
			this.ParentPanel.TabIndex = 35;
			// 
			// calibrate_btn
			// 
			this.calibrate_btn.Location = new System.Drawing.Point(447, 590);
			this.calibrate_btn.Name = "calibrate_btn";
			this.calibrate_btn.Size = new System.Drawing.Size(86, 37);
			this.calibrate_btn.TabIndex = 36;
			this.calibrate_btn.Text = "Calibrate";
			this.calibrate_btn.UseVisualStyleBackColor = true;
			this.calibrate_btn.Click += new System.EventHandler(this.Calibrate_btn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(963, 639);
			this.Controls.Add(this.calibrate_btn);
			this.Controls.Add(this.Dkoef);
			this.Controls.Add(this.deyes);
			this.Controls.Add(this.dexpected);
			this.Controls.Add(this.ParentPanel);
			this.Name = "MainForm";
			this.Text = "Eye finder";
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).EndInit();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ParentPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox fixedPicture;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox webCamPicture;
		private System.Windows.Forms.Button catchPicture_button;
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
		private System.Windows.Forms.Button calibrate_btn;
    }
}

