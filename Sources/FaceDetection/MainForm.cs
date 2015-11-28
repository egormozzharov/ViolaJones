using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
		private const string noseHaarCascadePath = @"Cascades\haarcascade_nose.xml";

		private string photoFileName;
		private CascadeClassifier _faceCascadeClassifier;
		private CascadeClassifier _eyeCascadeClassifier;
		private CascadeClassifier _mouthCascadeClassifier;
		private CascadeClassifier _noseCascadeClassifier;

		private Capture _capture = null;

		public MainForm()
		{
			InitializeComponent();

			_faceCascadeClassifier = new CascadeClassifier(faceHaarCascadePath);
			_eyeCascadeClassifier = new CascadeClassifier(eyeHaarCascadePath);
			_mouthCascadeClassifier = new CascadeClassifier(mouthHaarCascadePath);
			_noseCascadeClassifier = new CascadeClassifier(noseHaarCascadePath);
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
			Image<Gray, byte> frame = imageFrame.Convert<Gray, byte>();

			IEnumerable<Rectangle> faces = _faceCascadeClassifier.DetectMultiScale(frame, 1.1, 1, Size.Empty);
			if (faces.Any())
			{
				Rectangle face = faces.First();
				imageFrame.Draw(face, new Bgr(Color.BurlyWood));

				EyesDetection(imageFrame, frame, face);
				MouthDetection(imageFrame, frame, face);
				NoseDetection(imageFrame, frame, face);
			}

			fixedPicture.Image = imageFrame.Bitmap;
		}

		private void EyesDetection(Image<Bgr, Byte> imageFrame, Image<Gray, byte> frame, Rectangle face)
		{
			Rectangle eyesFrame = new Rectangle(face.X, face.Y, face.Width, face.Height / 2);
			imageFrame.Draw(eyesFrame, new Bgr(Color.Aqua), 4);

			Image<Gray, byte> eyesRegion = frame.Copy(eyesFrame);
			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X + face.X, eye.Y + face.Y, eye.Width, eye.Height);
				imageFrame.Draw(eyeAbsolute, new Bgr(Color.Aqua));
			}
		}

		private void MouthDetection(Image<Bgr, Byte> imageFrame, Image<Gray, byte> frame, Rectangle face)
		{
			int mouthFrameHeight = Convert.ToInt32(face.Height / 4);
			int mouthFrameY = face.Y + Convert.ToInt32(face.Height * 0.75);
			Rectangle mouthFrame = new Rectangle(face.X, mouthFrameY, face.Width, mouthFrameHeight);
			imageFrame.Draw(mouthFrame, new Bgr(Color.Red), 4);

			Image<Gray, byte> mouthRegion = frame.Copy(mouthFrame);
			IEnumerable<Rectangle> mouths = _mouthCascadeClassifier.DetectMultiScale(mouthRegion, 1.1, 1, Size.Empty);
			foreach (Rectangle mouth in mouths)
			{
				Rectangle mouthAbsolute = new Rectangle(mouth.X + face.X, mouth.Y + mouthFrameY, mouth.Width, mouth.Height);
				imageFrame.Draw(mouthAbsolute, new Bgr(Color.Red));
			}
		}

		private void NoseDetection(Image<Bgr, Byte> imageFrame, Image<Gray, byte> frame, Rectangle face)
		{
			int noseFrameHeight = Convert.ToInt32(face.Height * 0.25); 
			int noseFrameY = face.Y + Convert.ToInt32(face.Height * 0.5);
			Rectangle noseFrame = new Rectangle(face.X, noseFrameY, face.Width, noseFrameHeight);
			imageFrame.Draw(noseFrame, new Bgr(Color.BurlyWood), 4);

			Image<Gray, byte> noseRegion = frame.Copy(noseFrame);
			IEnumerable<Rectangle> noses = _noseCascadeClassifier.DetectMultiScale(noseRegion, 1.1, 2, Size.Empty);
			foreach (Rectangle nose in noses)
			{
				Rectangle eyeAbsolute = new Rectangle(nose.X + face.X, nose.Y + noseFrameY, nose.Width, nose.Height);
				imageFrame.Draw(eyeAbsolute, new Bgr(Color.BurlyWood));
			}
		}

		#endregion
	}
}
