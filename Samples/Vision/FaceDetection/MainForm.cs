// Accord.NET Sample Applications
// http://accord.googlecode.com
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using System;
using System.Drawing;
using System.Windows.Forms;
using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FaceDetection
{
    public partial class MainForm : Form
    {
		private WebCam_Capture.WebCamCapture WebCamCapture;
		private static string photoesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../../../Photoes");

	    private string photoFileName;
	    private Bitmap picture;

        HaarObjectDetector detector;

        public MainForm()
        {
            InitializeComponent();


            cbMode.DataSource = Enum.GetValues(typeof(ObjectDetectorSearchMode));
            cbScaling.DataSource = Enum.GetValues(typeof(ObjectDetectorScalingMode));

            cbMode.SelectedItem = ObjectDetectorSearchMode.Single;
            cbScaling.SelectedItem = ObjectDetectorScalingMode.GreaterToSmaller;

            toolStripStatusLabel1.Text = "Please select the detector options and click Detect to begin.";

            HaarCascade cascade = new FaceHaarCascade();
            detector = new HaarObjectDetector(cascade, 30);
        }

		private void LoadImageBtn_Click(object sender, EventArgs e)
		{
			photoFileName = this.PhotoName.Text;
			this.picture = (Bitmap)Image.FromFile(Path.Combine(photoesFolderPath, photoFileName), true);
			fixedPicture.Image = picture;
		}

		/// <summary>
		/// An image was capture
		/// </summary>
		/// <param name="source">control raising the event</param>
		/// <param name="e">WebCamEventArgs</param>
		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			// set the picturebox picture
			this.webCamPicture.Image = e.WebCamImage;
		}


        private void button1_Click(object sender, EventArgs e)
        {
			this.picture = (Bitmap)this.fixedPicture.Image;

            detector.SearchMode = (ObjectDetectorSearchMode)cbMode.SelectedValue;
            detector.ScalingMode = (ObjectDetectorScalingMode)cbScaling.SelectedValue;
            detector.ScalingFactor = 1.5f;
            detector.UseParallelProcessing = cbParallel.Checked;

            Stopwatch sw = Stopwatch.StartNew();


            // Process frame to detect objects
            Rectangle[] objects = detector.ProcessFrame(picture);


            sw.Stop();


            if (objects.Length > 0)
            {
                RectanglesMarker marker = new RectanglesMarker(objects, Color.Fuchsia);
				// this for webcamera images
				Pen pen = new Pen(Color.Red);
				Graphics g = fixedPicture.CreateGraphics();
				g.DrawRectangle(pen, objects.First());

				//fixedPicture.Image = marker.Apply(picture);
            }

            toolStripStatusLabel1.Text = string.Format("Completed detection of {0} objects in {1}.",
                objects.Length, sw.Elapsed);
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
    }
}
