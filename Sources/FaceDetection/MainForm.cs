using System;
using System.Drawing;
using System.Windows.Forms;

namespace FaceDetection
{
    public partial class MainForm : Form
    {
		private WebCam_Capture.WebCamCapture WebCamCapture;

	    private string photoFileName;
	    private Bitmap picture;

        public MainForm()
        {
            InitializeComponent();
        }

		#region EventActions

		/// <summary>
		/// An image was capture
		/// </summary>
		/// <param name="source">control raising the event</param>
		/// <param name="e">WebCamEventArgs</param>
		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			this.webCamPicture.Image = e.WebCamImage;
		}

        private void Detect_buttonClick(object sender, EventArgs e)
        {
			this.picture = (Bitmap)this.fixedPicture.Image;
        }

		private void startButton_Click(object sender, EventArgs e)
		{
			// change the capture time frame
			this.WebCamCapture.TimeToCapture_milliseconds = 2;

			// start the video capture. let the control handle the
			// frame numbers.
			this.WebCamCapture.Start(0);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// set the image capture size
			this.WebCamCapture.CaptureHeight = this.webCamPicture.Height;
			this.WebCamCapture.CaptureWidth = this.webCamPicture.Width;
		}

		private void catchPicture_button_Click(object sender, EventArgs e)
		{
			this.fixedPicture.Image = this.webCamPicture.Image;
		}

		#endregion
	}
}
