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
			this.cbMode = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbScaling = new System.Windows.Forms.ComboBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.cbParallel = new System.Windows.Forms.CheckBox();
			this.PhotoName = new System.Windows.Forms.TextBox();
			this.LoadImageBtn = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.catchPicture_button = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.webCamPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
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
			this.button1.Location = new System.Drawing.Point(10, 43);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(65, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Detect";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// fixedPicture
			// 
			this.fixedPicture.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.fixedPicture.Location = new System.Drawing.Point(12, 12);
			this.fixedPicture.Name = "fixedPicture";
			this.fixedPicture.Size = new System.Drawing.Size(287, 265);
			this.fixedPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.fixedPicture.TabIndex = 4;
			this.fixedPicture.TabStop = false;
			// 
			// cbMode
			// 
			this.cbMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbMode.FormattingEnabled = true;
			this.cbMode.Location = new System.Drawing.Point(126, 45);
			this.cbMode.Name = "cbMode";
			this.cbMode.Size = new System.Drawing.Size(113, 21);
			this.cbMode.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(83, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Mode:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(249, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Scaling:";
			// 
			// cbScaling
			// 
			this.cbScaling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbScaling.FormattingEnabled = true;
			this.cbScaling.Location = new System.Drawing.Point(300, 48);
			this.cbScaling.Name = "cbScaling";
			this.cbScaling.Size = new System.Drawing.Size(130, 21);
			this.cbScaling.TabIndex = 6;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 368);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(613, 22);
			this.statusStrip1.TabIndex = 8;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// cbParallel
			// 
			this.cbParallel.AutoSize = true;
			this.cbParallel.Checked = true;
			this.cbParallel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbParallel.Location = new System.Drawing.Point(252, 14);
			this.cbParallel.Name = "cbParallel";
			this.cbParallel.Size = new System.Drawing.Size(60, 17);
			this.cbParallel.TabIndex = 9;
			this.cbParallel.Text = "Parallel";
			this.cbParallel.UseVisualStyleBackColor = true;
			// 
			// PhotoName
			// 
			this.PhotoName.Location = new System.Drawing.Point(139, 14);
			this.PhotoName.Name = "PhotoName";
			this.PhotoName.Size = new System.Drawing.Size(100, 20);
			this.PhotoName.TabIndex = 10;
			// 
			// LoadImageBtn
			// 
			this.LoadImageBtn.Location = new System.Drawing.Point(10, 14);
			this.LoadImageBtn.Name = "LoadImageBtn";
			this.LoadImageBtn.Size = new System.Drawing.Size(65, 23);
			this.LoadImageBtn.TabIndex = 11;
			this.LoadImageBtn.Text = "LoadImage";
			this.LoadImageBtn.UseVisualStyleBackColor = true;
			this.LoadImageBtn.Click += new System.EventHandler(this.LoadImageBtn_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(77, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "photoPath";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.LoadImageBtn);
			this.panel1.Controls.Add(this.cbParallel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.cbScaling);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.PhotoName);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.cbMode);
			this.panel1.Location = new System.Drawing.Point(12, 283);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(433, 82);
			this.panel1.TabIndex = 13;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.catchPicture_button);
			this.panel2.Controls.Add(this.startButton);
			this.panel2.Location = new System.Drawing.Point(451, 283);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(150, 82);
			this.panel2.TabIndex = 14;
			// 
			// catchPicture_button
			// 
			this.catchPicture_button.Location = new System.Drawing.Point(58, 7);
			this.catchPicture_button.Name = "catchPicture_button";
			this.catchPicture_button.Size = new System.Drawing.Size(75, 23);
			this.catchPicture_button.TabIndex = 1;
			this.catchPicture_button.Text = "Catch";
			this.catchPicture_button.UseVisualStyleBackColor = true;
			this.catchPicture_button.Click += new System.EventHandler(this.catchPicture_button_Click);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(3, 7);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(49, 24);
			this.startButton.TabIndex = 0;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// webCamPicture
			// 
			this.webCamPicture.Location = new System.Drawing.Point(312, 12);
			this.webCamPicture.Name = "webCamPicture";
			this.webCamPicture.Size = new System.Drawing.Size(289, 265);
			this.webCamPicture.TabIndex = 15;
			this.webCamPicture.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(613, 390);
			this.Controls.Add(this.webCamPicture);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.fixedPicture);
			this.Name = "MainForm";
			this.Text = "Face Detection";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.fixedPicture)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.webCamPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox fixedPicture;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbScaling;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox cbParallel;
		private System.Windows.Forms.TextBox PhotoName;
		private System.Windows.Forms.Button LoadImageBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.PictureBox webCamPicture;
		private System.Windows.Forms.Button catchPicture_button;
    }
}

