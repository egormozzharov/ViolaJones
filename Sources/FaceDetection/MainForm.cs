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

			Image<Bgr, byte> frame = imageFrame;

			IEnumerable<Rectangle> faces = _faceCascadeClassifier.DetectMultiScale(frame, 1.1, 1, new Size(250, 250));
			if (faces.Any())
			{
				Color color = Color.BurlyWood;
				Rectangle face = faces.First();
				imageFrame.Draw(face, new Bgr(color));

				//draw face 
				Point faceUpperLeftPoint = new Point(face.X, face.Y - 10);
				Point faceUpperRightPoint = new Point(face.X + face.Width, face.Y - 10);
				DrawPoint(faceUpperLeftPoint, imageFrame, (color));
				DrawPoint(faceUpperRightPoint, imageFrame, (color));
				imageFrame.Draw(new LineSegment2D(new Point(Convert.ToInt32(face.X + face.Width * 0.5), face.Y),
					new Point(face.X + Convert.ToInt32(face.Width * 0.5), face.Y + face.Height)), new Bgr(color), 1);



				EyesDetection(imageFrame, frame, face);
				MouthDetection(imageFrame, frame, face);
				NoseDetection(imageFrame, frame, face);
			}


			fixedPicture.Image = imageFrame.Bitmap;
		}

		private void EyesDetection(Image<Bgr, Byte> imageFrame, Image<Bgr, byte> frame, Rectangle face)
		{
			Color color = Color.Aqua;
			Rectangle eyesFrame = new Rectangle(face.X, face.Y, face.Width, Convert.ToInt32(face.Height * 0.7));
			imageFrame.Draw(eyesFrame, new Bgr(color), 4);

			Image<Bgr, byte> eyesRegion = frame.Copy(eyesFrame);
			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			IList<Point> eyesCenters = new List<Point>();
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X + face.X, eye.Y + face.Y, eye.Width, eye.Height);
				imageFrame.Draw(eyeAbsolute, new Bgr(color));
				Point centerPoint = eyeAbsolute.Center();
				eyesCenters.Add(centerPoint);
				DrawPoint(centerPoint, imageFrame, (color));
			}
			imageFrame.Draw(new LineSegment2D(eyesCenters[0], eyesCenters[1]), new Bgr(Color.Black), 1);
			this.sideLongTiltLabel.Text = MathHelper.AngleWithHorizont(eyesCenters[0], eyesCenters[1]).ToString();
		}

		private void MouthDetection(Image<Bgr, Byte> imageFrame, Image<Bgr, byte> frame, Rectangle face)
		{
			Color color = Color.Red;
			int mouthFrameHeight = Convert.ToInt32(face.Height * 0.3);
			int mouthFrameY = face.Y + Convert.ToInt32(face.Height * 0.7);
			Rectangle mouthFrame = new Rectangle(face.X, mouthFrameY, face.Width, mouthFrameHeight);
			imageFrame.Draw(mouthFrame, new Bgr(color), 4);

			Image<Bgr, byte> mouthRegion = frame.Copy(mouthFrame);
			IEnumerable<Rectangle> mouths = _mouthCascadeClassifier.DetectMultiScale(mouthRegion, 1.1, 1, Size.Empty);
			foreach (Rectangle mouth in mouths)
			{
				Rectangle mouthAbsolute = new Rectangle(mouth.X + face.X, mouth.Y + mouthFrameY, mouth.Width, mouth.Height);
				imageFrame.Draw(mouthAbsolute, new Bgr(color));
				Point centerPoint = mouthAbsolute.Center();
				DrawPoint(centerPoint, imageFrame, color);
			}
		}

		private void NoseDetection(Image<Bgr, Byte> imageFrame, Image<Bgr, byte> frame, Rectangle face)
		{
			Color color = Color.BurlyWood;
			int noseFrameHeight = Convert.ToInt32(face.Height*0.25);
			int noseFrameY = face.Y + Convert.ToInt32(face.Height*0.5);
			Rectangle noseFrame = new Rectangle(face.X, noseFrameY, face.Width, noseFrameHeight);
			imageFrame.Draw(noseFrame, new Bgr(color), 4);

			Image<Bgr, byte> noseRegion = frame.Copy(noseFrame);
			IEnumerable<Rectangle> noses = _noseCascadeClassifier.DetectMultiScale(noseRegion, 1.1, 2, Size.Empty);
			foreach (Rectangle nose in noses)
			{
				Rectangle noseAbsolute = new Rectangle(nose.X + face.X, nose.Y + noseFrameY, nose.Width, nose.Height);
				imageFrame.Draw(noseAbsolute, new Bgr(color));
				Point centerPoint = noseAbsolute.Center();
				DrawPoint(centerPoint, imageFrame, color);
			}
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

		private Image<Bgr, byte> SkinMask(Image<Bgr, Byte> image, Rectangle face)
		{
			Image<Bgr, byte> faceMask = image.Copy(face);

			for (int j = 0; j < faceMask.Height; j++)
			{
				for (int i = 0; i < faceMask.Width; i++)
				{
					Bgr color = faceMask[j, i];
					if ((color.Red > 1.04*color.Green)
					    && (color.Green <= 8*color.Blue)
					    && (color.Green > 0.9*color.Blue
					        && (color.Red < 3.8*color.Green)))
					{

					}
					else
					{
						faceMask[j, i] = new Bgr(Color.Black);
					}
				}
			}
			return faceMask;
		}

		
	}

	public static class Extensions
	{
		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.Left + rect.Width / 2,
				rect.Top + rect.Height / 2);
		}
	}

	public static class MathHelper
	{
		public static double AngleWithHorizont(Point p1, Point p2)
		{
			return RadianToDegree(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X)) % 180;
		}

		private static double RadianToDegree(double radian)
		{
			var degree = radian * (180.0 / Math.PI);
			if (degree < 0)
				degree = 360 + degree;

			return degree;
		}
	}
}
