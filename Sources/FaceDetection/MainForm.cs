using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Interfaces;


namespace FaceDetection
{
	public partial class MainForm : Form
	{
		private WebCam_Capture.WebCamCapture WebCamCapture;
		private IDetectionService _detectionService;

		private Capture _capture = null;
		private Image _currentWebCamImage;

		public MainForm()
		{
			InitializeComponent();
			_detectionService = new DetectionService();
			SetComponentsSettings();
			SetWebcamSettings();
		}

		private void SetComponentsSettings()
		{
			//this.TopMost = true;
			this.WindowState = FormWindowState.Maximized;

			this.ParentPanel.Location = new Point(
			this.ClientSize.Width / 2 - this.ParentPanel.Size.Width / 2,
			this.ClientSize.Height / 2 - this.ParentPanel.Size.Height / 2);
			this.ParentPanel.Anchor = AnchorStyles.None;
		}

		private void SetWebcamSettings()
		{
			// set the image capture size
			this.WebCamCapture.CaptureHeight = this.webCamPicture.Height;
			this.WebCamCapture.CaptureWidth = this.webCamPicture.Width;

			// change the capture time frame
			this.WebCamCapture.TimeToCapture_milliseconds = 2;

			//start the video capture. let the control handle the
			//frame numbers.
			this.WebCamCapture.Start(0);
		}

		#region EventActions

		private void CatchPicture_button_Click(object sender, EventArgs e)
		{
			this.fixedPicture.Image = _currentWebCamImage;
		}

		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			this.webCamPicture.Image = e.WebCamImage;
			_currentWebCamImage = (Image)e.WebCamImage.Clone();
			Graphics g = Graphics.FromImage(e.WebCamImage);
			g.DrawRectangle(new Pen(Color.Red), new Rectangle(e.WebCamImage.Width / 2 - 30, 160, 80, 50));
			g.DrawRectangle(new Pen(Color.Red), new Rectangle(e.WebCamImage.Width / 2 + 120 - 30, 160, 80, 50));
		}

		private void Detect_buttonClick(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this.fixedPicture.Image);
			try
			{
				Rectangle face = _detectionService.GetFace(imageFrame);
				Image<Bgr, Byte> faceFrame = imageFrame.Copy(face);
				imageFrame = faceFrame;

				IList<Rectangle> detectedEyes = _detectionService.GetEyes(faceFrame);
				EyePair eyePair = GetEyeEdgesPair(detectedEyes);
				Point mouthCenterPoint = _detectionService.GetMouth(faceFrame);
				Point nosePoint = _detectionService.GetNose(faceFrame, eyePair);

				LineSegment2D lineBetweenEyes = new LineSegment2D(eyePair.LeftEye, eyePair.RightEye);
				LineSegment2D leftEyeToMouth = new LineSegment2D(eyePair.LeftEye, mouthCenterPoint);
				LineSegment2D rightEyeToMouth = new LineSegment2D(eyePair.RightEye, mouthCenterPoint);

				Point perpendicularPointFromNose = MathHelper.GetPerpendicularPoint(eyePair.LeftEye, eyePair.RightEye, nosePoint);
				Point pointBetweenEyes = lineBetweenEyes.MidPoint();

				double sideLongTilt = FaceRotation.GetSideLongTilt(eyePair);
				double rotationAroundVerticalOx = FaceRotation.GetRotationAroundVertical(pointBetweenEyes, perpendicularPointFromNose, lineBetweenEyes);
				double backForthAngle = FaceRotation.GetBackForthTilt(mouthCenterPoint, perpendicularPointFromNose, lineBetweenEyes);

				DisplayAnglesValues(sideLongTilt, rotationAroundVerticalOx, backForthAngle);

				DrawLines(new List<LineSegment2D>()
				{
					lineBetweenEyes, leftEyeToMouth, rightEyeToMouth, new LineSegment2D(mouthCenterPoint, perpendicularPointFromNose),
					new LineSegment2D(nosePoint, perpendicularPointFromNose)
				}, faceFrame);
				
				DrawingHelper.DrawPoint(pointBetweenEyes, imageFrame, Color.Red);

				fixedPicture.Image = imageFrame.Bitmap;
			}
			catch (Exception)
			{
				MessageBox.Show("Unable to find face");
			}
		}

		#endregion EventActions

		private void DisplayAnglesValues(double sideLongTilt, double rotationAroundVerticalOx, double backForthAngle)
		{
			this.sideLongTiltLabel.Text = sideLongTilt.ToString();
			this.rotationAroundVerticalOxLabel.Text = rotationAroundVerticalOx.ToString();
			this.backForthAngleLabel.Text = backForthAngle.ToString();
		}

		private void DrawLines(IEnumerable<LineSegment2D> lines, Image<Bgr, Byte> faceFrame)
		{
			foreach (LineSegment2D line in lines)
			{
				faceFrame.Draw(line, new Bgr(Color.Black), 1);
			}
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
				result.LeftEye = new Point(eyesRectangles[0].X, eyesRectangles[0].Y + eyesRectangles[0].Height / 2);
				result.RightEye = new Point(eyesRectangles[1].X + eyesRectangles[1].Width, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
			}
			else
			{
				result.LeftEye = new Point(eyesRectangles[1].X, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
				result.RightEye = new Point(eyesRectangles[0].X + eyesRectangles[0].Width, eyesRectangles[0].Y + eyesRectangles[0].Height / 2);
			}
			return result;
		}

	}
}
