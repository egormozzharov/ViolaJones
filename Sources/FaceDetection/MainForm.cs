using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
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

			// change the capture time frame
			this.WebCamCapture.TimeToCapture_milliseconds = 2;

			//start the video capture. let the control handle the
			//frame numbers.
			this.WebCamCapture.Start(0);

			this.firstParam.Text = 1.254.ToString();
			this.secondParam.Text = 2.ToString();
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
			IEnumerable<Rectangle> faces = _faceCascadeClassifier.DetectMultiScale(imageFrame, 1.1, 1, new Size(250, 250));
			if (faces.Any())
			{
				Color color = Color.BurlyWood;
				Rectangle face = faces.First();
				imageFrame.Draw(face, new Bgr(color));

				Image<Bgr, Byte> faceFrame = imageFrame.Copy(face);
				imageFrame = faceFrame;

				//middle line
				faceFrame.Draw(new LineSegment2D(new Point(Convert.ToInt32(face.Width * 0.5), 0),
					new Point(Convert.ToInt32(face.Width * 0.5),face.Height)), new Bgr(color), 1);

				IList<Point> eyesCenters = EyesDetection(faceFrame);
				Point mouthPoint = MouthDetection(faceFrame);
				EyePair eyePair = GetEyePair(eyesCenters);
				if (this.noseCheck.Checked)
				{
					IList<Point> nosePoints = NoseDetection(faceFrame, eyePair);
				}

				//koeffs
				this.sideLongTiltLabel.Text = MathHelper.AngleWithHorizont(eyesCenters[0], eyesCenters[1]).ToString();

				double dleft = MathHelper.LineLength(eyesCenters[0], mouthPoint);
				double dright = MathHelper.LineLength(eyesCenters[1], mouthPoint);

				double first = Convert.ToDouble(this.firstParam.Text);
				double second = Convert.ToDouble(this.secondParam.Text);
				double dExp = (((dleft + dright)/first)/second);
				this.dexpected.Text = dExp.ToString();

				double dEyes = MathHelper.LineLength(eyesCenters[0], eyesCenters[1]);
				this.deyes.Text = dEyes.ToString();

				this.Dkoef.Text = (dExp/dEyes).ToString();

				//drawing
				faceFrame.Draw(new LineSegment2D(eyesCenters[0], eyesCenters[1]), new Bgr(Color.Black), 1);
				faceFrame.Draw(new LineSegment2D(eyesCenters[0], mouthPoint), new Bgr(Color.Black), 1);
				faceFrame.Draw(new LineSegment2D(eyesCenters[1], mouthPoint), new Bgr(Color.Black), 1);
			}


			fixedPicture.Image = imageFrame.Bitmap;
		}


		private IList<Point> EyesDetection(Image<Bgr, Byte> faceFrame)
		{
			Color color = Color.Aqua;
			Rectangle eyesFrame = new Rectangle(0, 0, faceFrame.Width, Convert.ToInt32(faceFrame.Height * 0.7));
			faceFrame.Draw(eyesFrame, new Bgr(color), 4);

			Image<Bgr, byte> eyesRegion = faceFrame.Copy(eyesFrame);

			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			IList<Point> eyesCenters = new List<Point>();
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X, eye.Y, eye.Width, eye.Height);
				faceFrame.Draw(eyeAbsolute, new Bgr(color));
				Point centerPoint = eyeAbsolute.Center();
				eyesCenters.Add(centerPoint);
				DrawPoint(centerPoint, faceFrame, (color));
			}
			return eyesCenters;
		}

		private Point MouthDetection(Image<Bgr, Byte> faceFrame)
		{
			Color color = Color.Red;
			int mouthFrameHeight = Convert.ToInt32(faceFrame.Height * 0.3);
			int mouthFrameY = Convert.ToInt32(faceFrame.Height * 0.7);
			Rectangle mouthFrame = new Rectangle(0, mouthFrameY, faceFrame.Width, mouthFrameHeight);
			faceFrame.Draw(mouthFrame, new Bgr(color), 4);


			Image<Bgr, byte> mouthRegion = faceFrame.Copy(mouthFrame);
			IEnumerable<Rectangle> mouths = _mouthCascadeClassifier.DetectMultiScale(mouthRegion, 1.1, 1, Size.Empty);
			Point centerPoint = new Point();
			if (mouths.Any())
			{
				Rectangle mouth = mouths.First();
				Rectangle mouthAbsolute = new Rectangle(mouth.X, mouth.Y + mouthFrameY, mouth.Width, mouth.Height);
				faceFrame.Draw(mouthAbsolute, new Bgr(color));
				centerPoint = mouthAbsolute.Center();
				DrawPoint(centerPoint, faceFrame, color);
			}
			return centerPoint;
		}

		private IList<Point> NoseDetection(Image<Bgr, Byte> faceFrame, EyePair eyePair)
		{
			Color color = Color.BurlyWood;
			int noseFrameHeight = Convert.ToInt32(faceFrame.Height * 0.25);
			int noseFrameY = Convert.ToInt32(faceFrame.Height * 0.5);
			Rectangle noseFrame = new Rectangle(0, noseFrameY, faceFrame.Width, noseFrameHeight);
			faceFrame.Draw(noseFrame, new Bgr(color), 4);

			Image<Bgr, byte> noseRegion = faceFrame.Copy(noseFrame);
			IEnumerable<Rectangle> noses = _noseCascadeClassifier.DetectMultiScale(noseRegion, 1.1, 2, Size.Empty);
			IList<Point> centerPoints = new List<Point>();
			foreach (Rectangle nose in noses)
			{
				Rectangle noseAbsolute = new Rectangle(nose.X, nose.Y + noseFrameY, nose.Width, nose.Height);
				Point noseCenter = noseAbsolute.Center();
				centerPoints.Add(noseCenter);
				if (noseCenter.X >= eyePair.LeftEye.X && noseCenter.X <= eyePair.RightEye.X)
				{
					color = Color.Brown;
					faceFrame.Draw(noseAbsolute, new Bgr(color));
					DrawPoint(noseAbsolute.Center(), faceFrame, color);
				}
			}
			return centerPoints;
		}

		#endregion

		private void DrawCoordinates(Point centerPoint, Image<Bgr, Byte> imageFrame)
		{
			imageFrame.Draw(String.Format("x:{0}, y:{1}", centerPoint.X, centerPoint.Y), centerPoint,
					FontFace.HersheyComplex, 0.3, new Bgr(Color.Black));
		}

		private void DrawPoint(Point centerPoint, Image<Bgr, Byte> imageFrame, Color color)
		{
			imageFrame.Draw(new CircleF(centerPoint, 2), new Bgr(color));
			DrawCoordinates(centerPoint, imageFrame);
		}

		private EyePair GetEyePair(IList<Point> eyesCenters)
		{
			EyePair result = new EyePair();
			if (eyesCenters[0].X < eyesCenters[1].X)
			{
				result.LeftEye = eyesCenters[0];
				result.RightEye = eyesCenters[1];
			}
			else
			{
				result.LeftEye = eyesCenters[1];
				result.RightEye = eyesCenters[0];
			}
			return result;
		}

		private void SkinMask(ref Image<Bgr, Byte> faceFrame)
		{
			Image<Bgr, byte> faceMask = faceFrame;

			for (int j = 0; j < faceMask.Height; j++)
			{
				for (int i = 0; i < faceMask.Width; i++)
				{
					Bgr color = faceMask[j, i];
					if ((color.Red > 1.04*color.Green)
					    && (color.Green <= 8*color.Blue)
					    && (color.Green > 0.9*color.Blue
					    && (color.Red < 2.8*color.Green)))
					{
						faceMask[j, i] = new Bgr(Color.White);
					}
					else
					{
						faceMask[j, i] = new Bgr(Color.Black);
					}
				}
			}
		}	
	}
}
