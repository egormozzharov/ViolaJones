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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.catchPicture_button = new System.Windows.Forms.Button();
			this.webCamPicture = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.sideLongTiltLabel = new System.Windows.Forms.Label();
			this.dexpected = new System.Windows.Forms.Label();
			this.deyes = new System.Windows.Forms.Label();
			this.Dkoef = new System.Windows.Forms.Label();
			this.noseCheck = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rotationAroundVerticalOx = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.backForthAngle = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).BeginInit();
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
			this.fixedPicture.Location = new System.Drawing.Point(481, 22);
			this.fixedPicture.Name = "fixedPicture";
			this.fixedPicture.Size = new System.Drawing.Size(448, 381);
			this.fixedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.fixedPicture.TabIndex = 4;
			this.fixedPicture.TabStop = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 617);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(963, 22);
			this.statusStrip1.TabIndex = 8;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.catchPicture_button);
			this.panel2.Controls.Add(this.button1);
			this.panel2.Location = new System.Drawing.Point(12, 428);
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
			this.webCamPicture.Location = new System.Drawing.Point(12, 22);
			this.webCamPicture.Name = "webCamPicture";
			this.webCamPicture.Size = new System.Drawing.Size(447, 381);
			this.webCamPicture.TabIndex = 15;
			this.webCamPicture.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(23, 571);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(89, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Боковой наклон";
			// 
			// sideLongTiltLabel
			// 
			this.sideLongTiltLabel.AutoSize = true;
			this.sideLongTiltLabel.Location = new System.Drawing.Point(118, 571);
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
			// noseCheck
			// 
			this.noseCheck.AutoSize = true;
			this.noseCheck.Checked = true;
			this.noseCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.noseCheck.Location = new System.Drawing.Point(734, 534);
			this.noseCheck.Name = "noseCheck";
			this.noseCheck.Size = new System.Drawing.Size(80, 17);
			this.noseCheck.TabIndex = 29;
			this.noseCheck.Text = "noseCheck";
			this.noseCheck.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(274, 571);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 13);
			this.label2.TabIndex = 30;
			this.label2.Text = "Вокруг вертикальной оси";
			// 
			// rotationAroundVerticalOx
			// 
			this.rotationAroundVerticalOx.AutoSize = true;
			this.rotationAroundVerticalOx.Location = new System.Drawing.Point(382, 571);
			this.rotationAroundVerticalOx.Name = "rotationAroundVerticalOx";
			this.rotationAroundVerticalOx.Size = new System.Drawing.Size(0, 13);
			this.rotationAroundVerticalOx.TabIndex = 31;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(491, 571);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "ВпередНазад";
			// 
			// backForthAngle
			// 
			this.backForthAngle.AutoSize = true;
			this.backForthAngle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.backForthAngle.Location = new System.Drawing.Point(585, 571);
			this.backForthAngle.Name = "backForthAngle";
			this.backForthAngle.Size = new System.Drawing.Size(0, 13);
			this.backForthAngle.TabIndex = 33;
			this.backForthAngle.UseMnemonic = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(963, 639);
			this.Controls.Add(this.backForthAngle);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.rotationAroundVerticalOx);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.noseCheck);
			this.Controls.Add(this.Dkoef);
			this.Controls.Add(this.deyes);
			this.Controls.Add(this.dexpected);
			this.Controls.Add(this.sideLongTiltLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.webCamPicture);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.fixedPicture);
			this.Name = "MainForm";
			this.Text = "Eye finder";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox fixedPicture;
        private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox webCamPicture;
		private System.Windows.Forms.Button catchPicture_button;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label sideLongTiltLabel;
		private System.Windows.Forms.Label dexpected;
		private System.Windows.Forms.Label deyes;
		private System.Windows.Forms.Label Dkoef;
		private System.Windows.Forms.CheckBox noseCheck;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label rotationAroundVerticalOx;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label backForthAngle;
    }
}

