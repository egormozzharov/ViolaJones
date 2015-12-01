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
			try
			{
				IEnumerable<Rectangle> faces = _faceCascadeClassifier.DetectMultiScale(imageFrame, 1.1, 1, new Size(250, 250));
				if (faces.Any())
				{
					Rectangle face = faces.First();
					Image<Bgr, Byte> faceFrame = imageFrame.Copy(face);
					imageFrame = faceFrame;

					IList<Rectangle> detectedEyes = EyesDetection(faceFrame);
					EyePair eyePair = GetEyeEdgesPair(detectedEyes);
					Point mouthCenterPoint = MouthDetection(faceFrame);
					Point nosePoint = NoseDetection(faceFrame, eyePair);

					LineSegment2D lineBetweenEyes = new LineSegment2D(eyePair.LeftEye, eyePair.RightEye);
					LineSegment2D leftEyeToMouth = new LineSegment2D(eyePair.LeftEye, mouthCenterPoint);
					LineSegment2D rightEyeToMouth = new LineSegment2D(eyePair.RightEye, mouthCenterPoint);

					Point perpendicularPointFromNose = MathHelper.GetPerpendicularPoint(eyePair.LeftEye, eyePair.RightEye, nosePoint);
					Point pointBetweenEyes = lineBetweenEyes.MidPoint();

					//koeffs
					this.sideLongTiltLabel.Text = GetSideLongTilt(eyePair).ToString();
					this.rotationAroundVerticalOx.Text =
						Math.Abs(180 - GetRotationAroundVertical(pointBetweenEyes, perpendicularPointFromNose, lineBetweenEyes)).ToString();
					this.backForthAngle.Text =
						GetBackForthTilt(mouthCenterPoint, perpendicularPointFromNose, lineBetweenEyes).ToString();
					//drawing
					faceFrame.Draw(lineBetweenEyes, new Bgr(Color.Black), 1);
					faceFrame.Draw(leftEyeToMouth, new Bgr(Color.Black), 1);
					faceFrame.Draw(rightEyeToMouth, new Bgr(Color.Black), 1);
					faceFrame.Draw(new LineSegment2D(mouthCenterPoint, perpendicularPointFromNose), new Bgr(Color.BlueViolet), 1);
					//perpendicular line from nose
					faceFrame.Draw(new LineSegment2D(nosePoint, perpendicularPointFromNose), new Bgr(Color.Black), 1);
					DrawPoint(pointBetweenEyes, imageFrame, Color.Red);
				}
				fixedPicture.Image = imageFrame.Bitmap;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to find face");
			}
		}

		#endregion EventActions

		private double GetBackForthTilt(Point mouthCenterPoint, Point perpendicularPointFromNose, LineSegment2D lineBetweenEyes)
		{
			double result;
			LineSegment2D FD = new LineSegment2D(mouthCenterPoint, perpendicularPointFromNose);
			LineSegment2D AB = lineBetweenEyes;
			double r = Math.Abs(FD.Length)/Math.Abs(AB.Length);
			double expression = (r*Math.Cos(-0.24))/0.92;
			result = Math.Acos(expression) - 0.24;
			double degrees = MathHelper.RadianToDegree(result);
			return degrees;
		}

		private IList<Rectangle> EyesDetection(Image<Bgr, Byte> faceFrame)
		{
			Color color = Color.Aqua;
			Rectangle eyesFrame = new Rectangle(0, 0, faceFrame.Width, Convert.ToInt32(faceFrame.Height * 0.7));
			faceFrame.Draw(eyesFrame, new Bgr(color), 4);

			Image<Bgr, byte> eyesRegion = faceFrame.Copy(eyesFrame);

			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			IList<Rectangle> eyesDetected = new List<Rectangle>();
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X, eye.Y, eye.Width, eye.Height);
				eyesDetected.Add(eyeAbsolute);
				DrawDetectedObject(faceFrame, eyeAbsolute, color);
			}
			return eyesDetected;
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
			IList<Rectangle> mouthCandidates = new List<Rectangle>();
			foreach(Rectangle mouth in mouths)
			{
				Rectangle mouthAbsolute = new Rectangle(mouth.X, mouth.Y + mouthFrameY, mouth.Width, mouth.Height);
				mouthCandidates.Add(mouthAbsolute);
			}
			Rectangle detectedMouht = mouthCandidates.First();
			DrawDetectedObject(faceFrame, detectedMouht, color);
			return detectedMouht.Center();
		}

		private Point NoseDetection(Image<Bgr, Byte> faceFrame, EyePair eyePair)
		{
			Color color = Color.Brown;
			int noseFrameHeight = Convert.ToInt32(faceFrame.Height * 0.25);
			int noseFrameY = Convert.ToInt32(faceFrame.Height * 0.5);
			Rectangle noseFrame = new Rectangle(0, noseFrameY, faceFrame.Width, noseFrameHeight);
			faceFrame.Draw(noseFrame, new Bgr(color), 4);


			Image<Bgr, byte> noseRegion = faceFrame.Copy(noseFrame);
			IEnumerable<Rectangle> noses = _noseCascadeClassifier.DetectMultiScale(noseRegion, 1.1, 2, Size.Empty);
			IList<Rectangle> noseCandidates = new List<Rectangle>();
			foreach (Rectangle nose in noses)
			{
				Rectangle noseAbsolute = new Rectangle(nose.X, nose.Y + noseFrameY, nose.Width, nose.Height);
				Point noseCenter = noseAbsolute.Center();
				if (NoseIsBeetweenTheEyes(noseAbsolute, noseCenter, eyePair))
				{
					noseCandidates.Add(noseAbsolute);
				}
			}
			
			Rectangle detectedNose = GetNoseWithMaxSquare(noseCandidates);
			DrawDetectedObject(faceFrame, detectedNose, color);
			return detectedNose.Center();
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

		private EyePair GetEyeEdgesPair(IList<Rectangle> eyesRectangles)
		{
			EyePair result = new EyePair();
			if (eyesRectangles[0].X < eyesRectangles[1].X)
			{
				result.LeftEye = new Point(eyesRectangles[0].X, eyesRectangles[0].Y + eyesRectangles[0].Height/2);
				result.RightEye = new Point(eyesRectangles[1].X + eyesRectangles[1].Width, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
			}
			else
			{
				result.LeftEye = new Point(eyesRectangles[1].X, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
				result.RightEye = new Point(eyesRectangles[0].X + eyesRectangles[0].Width, eyesRectangles[0].Y + eyesRectangles[0].Height / 2);
			}
			return result;
		}

		private bool NoseIsBeetweenTheEyes(Rectangle noseAbsolute, Point noseCenter, EyePair eyePair)
		{
			return noseCenter.X >= eyePair.LeftEye.X && noseCenter.X <= eyePair.RightEye.X
				&& noseAbsolute.X >= eyePair.LeftEye.X && noseAbsolute.X <= eyePair.RightEye.X;
		}

		private Rectangle GetNoseWithMaxSquare(IList<Rectangle> noseCandidates)
		{
			int maxSquare = noseCandidates.Max(n => n.Square());
			return noseCandidates.First(n => n.Square() == maxSquare);
		}

		private double GetSideLongTilt(EyePair eyePair)
		{
			return MathHelper.AngleWithHorizont(eyePair.LeftEye, eyePair.RightEye);
		}

		private double GetRotationAroundVertical(Point e, Point d, LineSegment2D lineBetweenEyes)
		{
			LineSegment2D EDSegment = new LineSegment2D(e, d);
			double radians = -(EDSegment.Length/lineBetweenEyes.Length/0.55);
			double result = MathHelper.RadianToDegree(radians);
			return result;
		}


		#region Drawing

		private void DrawDetectedObject(Image<Bgr, Byte> faceFrame, Rectangle detectedObject, Color color)
		{
			faceFrame.Draw(detectedObject, new Bgr(color));
			DrawPoint(detectedObject.Center(), faceFrame, color);
		}

		private void DrawPoint(Point centerPoint, Image<Bgr, Byte> imageFrame, Color color)
		{
			imageFrame.Draw(new CircleF(centerPoint, 2), new Bgr(color));
			//DrawCoordinates(centerPoint, imageFrame);
		}

		private void DrawCoordinates(Point centerPoint, Image<Bgr, Byte> imageFrame)
		{
			imageFrame.Draw(String.Format("x:{0}, y:{1}", centerPoint.X, centerPoint.Y), centerPoint,
					FontFace.HersheyComplex, 0.3, new Bgr(Color.Black));
		}

		#endregion Drawing

	}
}
