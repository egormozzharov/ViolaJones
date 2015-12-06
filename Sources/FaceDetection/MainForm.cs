using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
		private ScreenCalibrationHelper _screenCalibrationHelper;
		private const string CalibrationFileName = "CalibrationList.ser";
		private int counterImage = 1;

		private Capture _capture = null;
		private Image _currentWebCamImage;

		public MainForm()
		{
			InitializeComponent();
			_detectionService = new DetectionService();
			_calibrationService = new CalibrationService(CalibrationFileName);
			_screenCalibrationHelper = new ScreenCalibrationHelper(_calibrationService.CalibrationInfoList);

			SetComponentsSettings();
			SetWebcamSettings();
			if (_screenCalibrationHelper.IsCalibrated)
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
			if (_screenCalibrationHelper.IsCalibrated)
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
				this.getCalibrationPoint_btn.Location = _screenCalibrationHelper.MoveScreenCalibrationPoint();
			}
		}

		private void SetWebcamSettings()
		{
			// set the image capture size
			this.WebCamCapture.CaptureHeight = 480;
			this.WebCamCapture.CaptureWidth = 640;

			// change the capture time frame
			this.WebCamCapture.TimeToCapture_milliseconds = 2;

			//start the video capture. let the control handle the
			//frame numbers.
			this.WebCamCapture.Start(0);
		}

		private void DrawStartPositionBorders(Image webCamImage)
		{
			Graphics g = Graphics.FromImage(webCamImage);
			g.DrawRectangle(new Pen(Color.Red), new Rectangle(0 + 190, 60, 170, 300));
			g.DrawLine(new Pen(Color.Red), 190, 160, 190 + 170, 160);
		}

		#region EventActions

		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			this.webCamPicture.Image = DrawingHelper.FixedSize(e.WebCamImage, webCamPicture.Width, webCamPicture.Height, true);
			_currentWebCamImage = (Image)e.WebCamImage.Clone();
			DrawStartPositionBorders(e.WebCamImage);
		}

		private void Calibrate_btn_Click(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)_currentWebCamImage);
			_currentWebCamImage.Save(String.Format("Images/Image{0}.jpg", counterImage++));
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				_calibrationService.CalibrationInfoList.Add(new CalibrationItem()
				{
					EyePoint = detectionResult.EyeCentersPair.RightEye,
					ScreenPoint = _screenCalibrationHelper.CurrentScreenCalibrationPoint,
					BridgeCoordinatesAbsolute = detectionResult.BridgeNosePoint,
				});
				ChangeCalibrationButtonsParameters();
				AnglesLogic(detectionResult, imageFrame);
				DrawDetectedObjects(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void StartCalibration_btn_Click(object sender, EventArgs e)
		{
			_calibrationService.CalibrationInfoList.Clear();
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
			if (_screenCalibrationHelper.IsCalibrated)
			{
				_calibrationService.SaveCalibrationList(CalibrationFileName);
			}
			MessageBox.Show("Saved");
		}

		private void Analize_btn_Click(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this._currentWebCamImage);
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				AnglesLogic(detectionResult, imageFrame);
				ViewDetectionLogic(detectionResult, imageFrame);
				DrawDetectedObjects(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void DrawDetectedObjects(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			LineSegment2D lineBetweenEyes = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye);
			LineSegment2D leftEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.MouthCenterPoint);
			LineSegment2D rightEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.RightEye, detectionResult.MouthCenterPoint);
			Point bridgeNosePoint = detectionResult.BridgeNosePoint;
			Point pointBetweenEyes = lineBetweenEyes.MidPoint();
			DrawingHelper.DrawDetectedObjects(imageFrame, detectionResult);

			DrawingHelper.DrawLines(new List<LineSegment2D>()
				{
					lineBetweenEyes, leftEyeToMouth, rightEyeToMouth, new LineSegment2D(detectionResult.MouthCenterPoint, bridgeNosePoint),
					new LineSegment2D(detectionResult.NosePoint, bridgeNosePoint)
				}, imageFrame);

			DrawingHelper.DrawPoint(pointBetweenEyes, imageFrame, Color.Red);
		}

		private void AnglesLogic(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			LineSegment2D lineBetweenEyes = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye);
			Point bridgeNosePoint = detectionResult.BridgeNosePoint;
			Point pointBetweenEyes = lineBetweenEyes.MidPoint();

			double sideLongTilt = FaceRotation.GetSideLongTilt(detectionResult.EyeEdgesPair);
			double rotationAroundVerticalOx = FaceRotation.GetRotationAroundVertical(pointBetweenEyes, bridgeNosePoint, lineBetweenEyes);
			double backForthAngle = FaceRotation.GetBackForthTilt(detectionResult.MouthCenterPoint, bridgeNosePoint, lineBetweenEyes);
			DisplayAnglesValues(sideLongTilt, rotationAroundVerticalOx, backForthAngle);
		}

		private void ViewDetectionLogic(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			Point rightEye = detectionResult.EyeCentersPair.RightEye;
			RecalculateCoordinates(detectionResult, ref rightEye);
			
			//draw detected eye poit 
			DrawingHelper.DrawPoint(rightEye, imageFrame, Color.Blue);
			//draw all calibrated points
			DrawingHelper.DrawPoints(_calibrationService.CalibrationInfoList.Select(item => item.EyePoint), imageFrame, Color.DarkViolet);
			//draw base line
			DrawingHelper.DrawLine(_calibrationService.FBaseLine, imageFrame, Color.Crimson);

			
			double eyePointAngle = _calibrationService.GetFLineAngle(rightEye);
			double eyePointRadius = new LineSegment2D(rightEye, _calibrationService.FCenterPoint).Length;
			PolarCoordinate eyePolarCoordinate = new PolarCoordinate()
			{
				Angle = eyePointAngle,
				Radius = eyePointRadius,
			};
			//screen angle calculation
			double screenAngle = _calibrationService.GetScreenAngle(eyePolarCoordinate);
			Point screenPoint = MathHelper.ConvertPolarToRectangularCoordinaty(new PolarCoordinate()
			{
				Angle = screenAngle,
				Radius = 200,
			}, _calibrationService.SCenterPoint);
			this.SAngleLabel.Text = screenAngle.ToString();
			DrawingHelper.DrawLine(backgroundPanel, screenPoint, _calibrationService.SCenterPoint);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.SCenterPoint);
			ParentPanel.Hide();
		}

		private void RecalculateCoordinates(DetectionResult detectionResult, ref Point rightEye)
		{
			_calibrationService.RecalculateCalibration();
			rightEye = _calibrationService.RecalculateCalibratedPoint(detectionResult.BridgeNosePoint, rightEye);
		}

		private void ShowPanels_btn_Click(object sender, EventArgs e)
		{
			this.ParentPanel.Show();
		}
	}
}
