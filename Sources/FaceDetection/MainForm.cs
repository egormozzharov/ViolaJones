using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace FaceDetection
{
	public partial class MainForm : Form
	{
		private WebCam_Capture.WebCamCapture WebCamCapture;
		private const string faceHaarCascadePath = @"Cascades\haarcascade_frontalface_alt.xml";
		private const string eyeHaarCascadePath = @"Cascades\haarcascade_eye_treeglasses.xml";
		private const string mouthHaarCascadePath = @"Cascades\haarcascade_mouth.xml";

		private string photoFileName;
		private CascadeClassifier _faceCascadeClassifier;
		private CascadeClassifier _eyeCascadeClassifier;
		private CascadeClassifier _mouthCascadeClassifier;

		private Capture _capture = null;

		public MainForm()
		{
			InitializeComponent();

			_faceCascadeClassifier = new CascadeClassifier(faceHaarCascadePath);
			_eyeCascadeClassifier = new CascadeClassifier(eyeHaarCascadePath);
			_mouthCascadeClassifier = new CascadeClassifier(mouthHaarCascadePath);
		}

		#region EventActions

		private void MainForm_Load(object sender, EventArgs e)
		{
			_faceCascadeClassifier = new CascadeClassifier(faceHaarCascadePath);
			// set the image capture size
			this.WebCamCapture.CaptureHeight = this.webCamPicture.Height;
			this.WebCamCapture.CaptureWidth = this.webCamPicture.Width;
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			// change the capture time frame
			this.WebCamCapture.TimeToCapture_milliseconds = 2;

			//start the video capture. let the control handle the
			//frame numbers.
			this.WebCamCapture.Start(0);
		}

		private void CatchPicture_button_Click(object sender, EventArgs e)
		{
			this.fixedPicture.Image = this.webCamPicture.Image;
		}

		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			this.webCamPicture.Image = e.WebCamImage;
		}

		private void Detect_buttonClick(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this.fixedPicture.Image);
			var grayframe = imageFrame.Convert<Gray, byte>();

			IEnumerable<Rectangle> faces = _faceCascadeClassifier.DetectMultiScale(grayframe, 1.1, 1, Size.Empty);
			foreach (Rectangle face in faces)
			{
				imageFrame.Draw(face, new Bgr(Color.BurlyWood));
			}


			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(grayframe, 1.1, 2, Size.Empty);
			foreach (Rectangle eye in eyes)
			{
				imageFrame.Draw(eye, new Bgr(Color.Aqua));
			}

			IEnumerable<Rectangle> mouths = _mouthCascadeClassifier.DetectMultiScale(grayframe, 1.1, 1, Size.Empty);
			foreach (Rectangle mouth in mouths)
			{
				imageFrame.Draw(mouth, new Bgr(Color.Aqua));
			}

			fixedPicture.Image = imageFrame.Bitmap;
		}

		#endregion
	}
}
