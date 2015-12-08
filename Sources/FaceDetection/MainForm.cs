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
			_calibrationService = new CalibrationService();
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

		private void DrawCenterCalibratedPoint(Image webCamImage)
		{
			CalibrationItem centerCalibrationItem = _calibrationService.CalibrationInfoList.LastOrDefault();
			if (centerCalibrationItem != null)
			{
				Point rightEye = centerCalibrationItem.RightEyePoint;
				Point leftEye = centerCalibrationItem.LeftEyePoint;
				Point bridgeNose = centerCalibrationItem.BridgeCoordinatesAbsolute;
				DrawingHelper.DrawPoint(webCamImage, rightEye, Color.Blue, 7);
				DrawingHelper.DrawPoint(webCamImage, leftEye, Color.Blue, 7);
				DrawingHelper.DrawPoint(webCamImage, bridgeNose, Color.Red, 7);
			}
		}

		private void SetComponentsSettings()
		{
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
			this.WebCamCapture.CaptureHeight = 480;
			this.WebCamCapture.CaptureWidth = 640;
			this.WebCamCapture.TimeToCapture_milliseconds = 2;
			this.WebCamCapture.Start(0);
		}

		#region EventActions

		private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
		{
			this.webCamPicture.Image = e.WebCamImage;
			_currentWebCamImage = (Image)e.WebCamImage.Clone();
			DrawCenterCalibratedPoint(e.WebCamImage);
		}

		private void Calibrate_btn_Click(object sender, EventArgs e)
		{
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)_currentWebCamImage);
			//_currentWebCamImage.Save(String.Format("Images/Image{0}.jpg", counterImage++));
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				_calibrationService.CalibrationInfoList.Add(new CalibrationItem()
				{
					RightEyePoint = detectionResult.EyeCentersPair.RightEye,
					LeftEyePoint = detectionResult.EyeCentersPair.LeftEye,
					ScreenPoint = _screenCalibrationHelper.CurrentScreenCalibrationPoint,
					BridgeCoordinatesAbsolute = detectionResult.BridgeNosePoint,
				});
				ChangeCalibrationButtonsParameters();
				AnglesLogic(detectionResult);
				DrawDetectedObjects(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
			}
		}

		private void StartCalibration_btn_Click(object sender, EventArgs e)
		{
			_calibrationService.CalibrationInfoList.Clear();
			ChangeCalibrationButtonsParameters();
		}

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
			DrawingHelper.Clear(backgroundPanel);
			Image<Bgr, Byte> imageFrame = new Image<Bgr, Byte>((Bitmap)this._currentWebCamImage);
			DetectionResult detectionResult = _detectionService.GetDetectionResult(imageFrame);
			if (detectionResult.Status == DetectionStatus.Success)
			{
				AnglesLogic(detectionResult);
				ViewDetectionLogic(detectionResult);
				DrawDetectedObjects(detectionResult, imageFrame);
				fixedPicture.Image = imageFrame.Bitmap;
				HideParantPanel();
			}
		}

		private void TestButton_Click(object sender, EventArgs e)
		{
			Point rightEye = new Point(Convert.ToInt32(xDetectedEye.Text), Convert.ToInt32(yDetectedEye.Text));
			DrawingHelper.Clear(backgroundPanel);
			ViewDetectionLogic(detectionResult: new DetectionResult()
			{
				EyeCentersPair = new EyePair()
				{
					RightEye = rightEye,
				}
			});
			HideParantPanel();
		}

		#endregion EventActions



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

		private void AnglesLogic(DetectionResult detectionResult)
		{
			LineSegment2D lineBetweenEyes = new LineSegment2D(detectionResult.EyeEdgesPair.LeftEye, detectionResult.EyeEdgesPair.RightEye);
			Point bridgeNosePoint = detectionResult.BridgeNosePoint;
			Point pointBetweenEyes = lineBetweenEyes.MidPoint();

			double sideLongTilt = FaceRotation.GetSideLongTilt(detectionResult.EyeEdgesPair);
			double rotationAroundVerticalOx = FaceRotation.GetRotationAroundVertical(pointBetweenEyes, bridgeNosePoint, lineBetweenEyes);
			double backForthAngle = FaceRotation.GetBackForthTilt(detectionResult.MouthCenterPoint, bridgeNosePoint, lineBetweenEyes);
			DisplayAnglesValues(sideLongTilt, rotationAroundVerticalOx, backForthAngle);
		}

		private void ViewDetectionLogic(DetectionResult detectionResult)
		{
			Point rightEye = detectionResult.EyeCentersPair.RightEye;
			//RecalculateCoordinates(detectionResult, ref rightEye);
			
			//draw all calibrated points
			DrawPointsWithColors();
			//draw detected eye poit 
			DrawingHelper.DrawPoint(backgroundPanel, rightEye, Color.Brown, 1);
			//draw base line
			DrawingHelper.DrawLine(backgroundPanel, _calibrationService.FBaseLine.P1, _calibrationService.FBaseLine.P2, Color.Crimson);

			
			PolarCoordinate eyePolarCoordinate = new PolarCoordinate()
			{
				Angle = _calibrationService.GetFLineAngle(rightEye),
				Radius = new LineSegment2D(rightEye, _calibrationService.FCenterPoint).Length,
			};
			PolarCoordinate screenPolarCoordinate = _calibrationService.GetScreenPolarCoordinate(eyePolarCoordinate);
			Point screenPoint = MathHelper.ConvertPolarToRectangularCoordinaty(screenPolarCoordinate, _calibrationService.SCenterPoint);
			this.SAngleLabel.Text = screenPolarCoordinate.Angle.ToString();
			DrawingHelper.DrawLine(backgroundPanel, screenPoint, _calibrationService.SCenterPoint, Color.Red);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.SCenterPoint, Color.Brown);
		}

		private void DrawPointsWithColors()
		{
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[0].RightEyePoint, Color.Red, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[1].RightEyePoint, Color.Orange, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[2].RightEyePoint, Color.Yellow, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[3].RightEyePoint, Color.Green, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[4].RightEyePoint, Color.LightBlue, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[5].RightEyePoint, Color.Blue, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[6].RightEyePoint, Color.Violet, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[7].RightEyePoint, Color.BlueViolet, 1);
			DrawingHelper.DrawPoint(backgroundPanel, _calibrationService.CalibrationInfoList[8].RightEyePoint, Color.Black, 1);
		}

		private void RecalculateCoordinates(DetectionResult detectionResult, ref Point rightEye)
		{
			_calibrationService.RecalculateCalibration();
			rightEye = _calibrationService.RecalculateCalibratedPoint(detectionResult.BridgeNosePoint, rightEye);
		}

		private void ShowPanels_btn_Click(object sender, EventArgs e)
		{
			ShowParentPanel();
		}

		private void HidePanels_btn_Click(object sender, EventArgs e)
		{
			HideParantPanel();
		}

		private void HideParantPanel()
		{
			this.ParentPanel.Hide();
			this.ShowPanels_btn.Visible = true;
			this.HidePanels_btn.Visible = false;
		}

		private void ShowParentPanel()
		{
			this.ParentPanel.Show();
			this.ShowPanels_btn.Visible = false;
			this.HidePanels_btn.Visible = true;
		}
	}
}
