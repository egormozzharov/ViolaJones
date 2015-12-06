using System.Collections.Generic;
using System.Drawing;
using FaceDetection.Models;

namespace FaceDetection.Helpers
{
	public class ScreenCalibrationHelper
	{
		private const int PointsNumber = 9;
		private readonly IList<CalibrationItem> _calibratoinList;
		private int _currentSceenCalibrationPoint = -1;

		public ScreenCalibrationHelper(IList<CalibrationItem> calibrationList)
		{
			this._calibratoinList = calibrationList;
		}

		public Point CurrentScreenCalibrationPoint
		{
			get
			{
				return ScreenCalibrationPoints[_currentSceenCalibrationPoint];
			}
		}

		public Point MoveScreenCalibrationPoint()
		{
			_currentSceenCalibrationPoint++;
			return ScreenCalibrationPoints[_currentSceenCalibrationPoint];
		}

		public bool HasNextCalibrationPoint
		{
			get { return IsCalibrated; }
		}

		public bool IsCalibrated
		{
			get { return _calibratoinList.Count == PointsNumber; }
		}

		private Rectangle Screen
		{
			get
			{
				return System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			}
		}

		private IList<Point> ScreenCalibrationPoints
		{
			get
			{
				IList<Point> sPoints = new List<Point>();
				sPoints.Add(new Point((Screen.X + Screen.Width) - PaddingWidth, (Screen.Y + Screen.Height / 2)));
				sPoints.Add(new Point((Screen.X + Screen.Width) - PaddingWidth, Screen.Y));
				sPoints.Add(new Point((Screen.X + Screen.Width / 2), Screen.Y));
				sPoints.Add(new Point(Screen.X, Screen.Y));
				sPoints.Add(new Point(Screen.X, Screen.Y + Screen.Height / 2));
				sPoints.Add(new Point(Screen.X, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add(new Point(Screen.X + Screen.Width / 2, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add(new Point((Screen.X + Screen.Width) - PaddingWidth, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add(new Point(Screen.X + Screen.Width / 2, Screen.Y + Screen.Height / 2));
				return sPoints;
			}
		}

		private int PaddingHeight
		{
			get { return 50; }
		}

		private int PaddingWidth
		{
			get { return 50; }
		}
	}
}
