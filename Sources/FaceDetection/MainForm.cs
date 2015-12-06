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
				_calibrationService.CalibrationInfoList.Add(new CalibrationItem()
				{
					EyePoint = detectionResult.EyeCentersPair.RightEye,
					ScreenPoint = _screenCalibrationHelper.CurrentScreenCalibrationPoint,
					BridgeCoordinatesAbsolute = detectionResult.BridgeNosePoint,
				});
				ChangeCalibrationButtonsParameters();
				AnglesLogic(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
			if (_screenCalibrationHelper.IsCalibrated)
			{
				//Recalculate calibrated coordinates based on NoseBridge shifting.
				_calibrationService.RecalculateCalibration();
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
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this.fixedPicture.Image);
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				AnglesLogic(detectionResult, imageFrame);
				ViewDetectionLogic(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void AnglesLogic(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			LineSegment2D lineBetweenEyes = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye);
			LineSegment2D leftEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.MouthCenterPoint);
			LineSegment2D rightEyeToMouth = new LineSegment2D(detectionResult.EyeEdgesPair.RightEye, detectionResult.MouthCenterPoint);

			Point bridgeNosePoint = detectionResult.BridgeNosePoint;
			Point pointBetweenEyes = lineBetweenEyes.MidPoint();

			double sideLongTilt = FaceRotation.GetSideLongTilt(detectionResult.EyeEdgesPair);
			double rotationAroundVerticalOx = FaceRotation.GetRotationAroundVertical(pointBetweenEyes, bridgeNosePoint, lineBetweenEyes);
			double backForthAngle = FaceRotation.GetBackForthTilt(detectionResult.MouthCenterPoint, bridgeNosePoint, lineBetweenEyes);

			DisplayAnglesValues(sideLongTilt, rotationAroundVerticalOx, backForthAngle);
			DrawingHelper.DrawDetectedObjects(imageFrame, detectionResult);

			DrawingHelper.DrawLines(new List<LineSegment2D>()
				{
					lineBetweenEyes, leftEyeToMouth, rightEyeToMouth, new LineSegment2D(detectionResult.MouthCenterPoint, bridgeNosePoint),
					new LineSegment2D(detectionResult.NosePoint, bridgeNosePoint)
				}, imageFrame);

			DrawingHelper.DrawPoint(pointBetweenEyes, imageFrame, Color.Red);
		}

		private void ViewDetectionLogic(DetectionResult detectionResult, Image<Bgr, Byte> imageFrame)
		{
			//draw all calibrated points
			DrawingHelper.DrawPoints(_calibrationService.CalibrationInfoList.Select(item => item.EyePoint), imageFrame, Color.Chartreuse);
			//draw base line
			DrawingHelper.DrawLine(_calibrationService.FBaseLine, imageFrame, Color.Crimson);

			Point rightEye = detectionResult.EyeCentersPair.RightEye;
			double eyePointAngle = _calibrationService.GetFLineAngle(rightEye);
			double eyePointRadius = new LineSegment2D(rightEye, _calibrationService.FCenterPoint).Length;
			PolarCoordinate eyePolarCoordinate = new PolarCoordinate()
			{
				Angle = eyePointAngle,
				Radius = eyePointRadius,
			};
			double screenAngle = _calibrationService.GetScreenAngle(eyePolarCoordinate);
			this.fiF.Text = eyePointAngle.ToString();
		}
	}
}
