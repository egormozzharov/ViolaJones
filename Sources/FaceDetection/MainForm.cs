using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Helpers;
using FaceDetection.Implementations;
using FaceDetection.Interfaces;
using FaceDetection.Models;


namespace FaceDetection
{
	public partial class MainForm : Form
	{
		private WebCam_Capture.WebCamCapture WebCamCapture;
		private IDetectionService _detectionService;
		private CalibrationService _calibrationService;
		private const string CalibrationFileName = "CalibrationList.ser";

		private Capture _capture = null;
		private Image _currentWebCamImage;

		public MainForm()
		{
			InitializeComponent();
			_detectionService = new DetectionService();
			_calibrationService = new CalibrationService(CalibrationFileName);
			SetComponentsSettings();
			SetWebcamSettings();
			if (_calibrationService.IsCalibrated)
			{
				this.saveCalibration_btn.Visible = true;
			}
			else
			{
				this.saveCalibration_btn.Visible = false;
			}
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

		private void ChangeCalibrationButtonsParameters()
		{
			if (_calibrationService.IsCalibrated)
			{
				this.analize_btn.Visible = true;
				this.getCalibrationPoint_btn.Visible = false;
				this.saveCalibration_btn.Visible = true;
			}
			else
			{
				this.analize_btn.Visible = false;
				this.getCalibrationPoint_btn.Visible = true;
				this.saveCalibration_btn.Visible = false;
				this.getCalibrationPoint_btn.Location = _calibrationService.MoveScreenCalibrationPoint();
			}
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

		private void DrawStartPositionBorders(Image webCamImage)
		{
			Graphics g = Graphics.FromImage(webCamImage);
			//g.DrawRectangle(new Pen(Color.Red), new Rectangle(webCamImage.Width / 2 - 30, 160, 80, 50));
			//g.DrawRectangle(new Pen(Color.Red), new Rectangle(webCamImage.Width / 2 + 120 - 30, 160, 80, 50));
			g.DrawRectangle(new Pen(Color.Red), new Rectangle(0 + 190, 60, 170, 300));
			g.DrawLine(new Pen(Color.Red), 190, 160, 190 + 170, 160);

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
			DrawStartPositionBorders(e.WebCamImage);
		}

		private void Calibrate_btn_Click(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)_currentWebCamImage);
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				_calibrationService.CalibrationList.Add(new CalibrationItem()
				{
					EyePoint = detectionResult.EyeCentersPair.RightEye,
					ScreenPoint = _calibrationService.CurrentScreenCalibrationPoint,

				});
				ChangeCalibrationButtonsParameters();
				AnglesLogic(detectionResult, ref imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void StartCalibration_btn_Click(object sender, EventArgs e)
		{
			_calibrationService.CalibrationList.Clear();
			ChangeCalibrationButtonsParameters();
		}

		#endregion EventActions

		private void DisplayAnglesValues(double sideLongTilt, double rotationAroundVerticalOx, double backForthAngle)
		{
			this.sideLongTiltLabel.Text = sideLongTilt.ToString();
			this.rotationAroundVerticalOxLabel.Text = rotationAroundVerticalOx.ToString();
			this.backForthAngleLabel.Text = backForthAngle.ToString();
		}

		private void SaveCalibration_Click(object sender, EventArgs e)
		{
			if (_calibrationService.IsCalibrated)
			{
				_calibrationService.SaveCalibrationList(CalibrationFileName);
			}
			MessageBox.Show("Saved");
		}

		private void Analize_btn_Click(object sender, EventArgs e)
		{
			var r = _calibrationService.PolarCoordinatesS;

			LineSegment2D baseLine = new LineSegment2D(new Point(0, 2), new Point(2, 2));
			double firstAngle = MathHelper.AngleBetweenLines(new LineSegment2D(new Point(0, 0), new Point(2, 2)),baseLine);
			double secondAngle = MathHelper.AngleBetweenLines(new LineSegment2D(new Point(2, 2), new Point(4, 4)), baseLine);

			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this.fixedPicture.Image);
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				AnglesLogic(detectionResult, ref imageFrame);
				ViewDetectionLogic(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void ViewDetectionLogic(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			Point centerFPoint = _calibrationService.FCenterPoint;
			Point detectedRightEye = detectionResult.EyeCentersPair.RightEye;
			foreach (Point eyePoint in _calibrationService.CalibrationList.Select(item => item.EyePoint))
			{
				DrawingHelper.DrawPoint(eyePoint, imageFrame, Color.Chartreuse);
			}
			
			DrawingHelper.DrawPoint(detectedRightEye, imageFrame, Color.Blue);
			//Horizontal line throuth centerFPoint
			DrawingHelper.DrawLine(new LineSegment2D(new Point(0, centerFPoint.Y),
				new Point(_calibrationService.Screen.Width, centerFPoint.Y)), imageFrame, Color.Red);
			//Radius
			LineSegment2D radius = new LineSegment2D(centerFPoint, detectedRightEye);
			DrawingHelper.DrawLine(radius, imageFrame, Color.Red);
			//Catet to horizontal line (opposing)
			LineSegment2D catetOpposint = new LineSegment2D(detectedRightEye, new Point(detectedRightEye.X, centerFPoint.Y));
			DrawingHelper.DrawLine(catetOpposint, imageFrame, Color.Red);
			double fiFrameAngle = Math.Asin(catetOpposint.Length / radius.Length);
			this.fiF.Text = fiFrameAngle.ToString();
		}

		private void AnglesLogic(DetectionResult detectionResult, ref Image<Bgr, Byte> imageFrame)
		{
			LineSegment2D lineBetweenEyes = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye);
			LineSegment2D leftEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.MouthCenterPoint);
			LineSegment2D rightEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.RightEye, detectionResult.MouthCenterPoint);

			Point perpendicularPointFromNose = MathHelper.GetPerpendicularPoint(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye,
				detectionResult.NosePoint);
			Point pointBetweenEyes = lineBetweenEyes.MidPoint();

			double sideLongTilt = FaceRotation.GetSideLongTilt(detectionResult.EyeEdgesPair);
			double rotationAroundVerticalOx = FaceRotation.GetRotationAroundVertical(pointBetweenEyes, perpendicularPointFromNose, lineBetweenEyes);
			double backForthAngle = FaceRotation.GetBackForthTilt(detectionResult.MouthCenterPoint, perpendicularPointFromNose, lineBetweenEyes);

			DisplayAnglesValues(sideLongTilt, rotationAroundVerticalOx, backForthAngle);

			imageFrame = DrawingHelper.GetImageFrame(imageFrame, detectionResult.Face);
			DrawingHelper.DrawDetectedObjects(imageFrame, detectionResult);

			DrawingHelper.DrawLines(new List<LineSegment2D>()
				{
					lineBetweenEyes, leftEyeToMouth, rightEyeToMouth, new LineSegment2D(detectionResult.MouthCenterPoint, perpendicularPointFromNose),
					new LineSegment2D(detectionResult.NosePoint, perpendicularPointFromNose)
				}, imageFrame);

			DrawingHelper.DrawPoint(pointBetweenEyes, imageFrame, Color.Red);
		}
	}
}
