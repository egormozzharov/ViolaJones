using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV.Structure;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	public class CalibrationService
	{
		private const int PointsNumber = 9;
		private readonly SerializationService _serializationService;
		private int _currentSceenCalibrationPoint = -1;

		public CalibrationService(string calibrationFileName)
		{
			_serializationService = new SerializationService();
			CalibrationList = TryLoadCalibrationList(calibrationFileName);
		}

		public IList<CalibrationItem> CalibrationList { get; set; }

		public Point CurrentScreenCalibrationPoint
		{
			get
			{
				return ScreenCalibrationPoints[_currentSceenCalibrationPoint];
			}
		}

		public Point FCenterPoint
		{
			get { return CalibrationList[8].EyePoint; }
		}

		public Point FPoint1
		{
			get { return CalibrationList[0].EyePoint; }
		}

		public Point SCenterPoint
		{
			get { return ScreenCalibrationPoints[8]; }
		}

		public Point SPoint1
		{
			get { return ScreenCalibrationPoints[0]; }
		}

		public void SaveCalibrationList(string calibrationFileName)
		{
			_serializationService.Serialize(CalibrationList, calibrationFileName);
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
			get { return CalibrationList.Count == PointsNumber; }
		}

		public Rectangle Screen
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

		private IList<CalibrationItem> TryLoadCalibrationList(string calibrationFilePath)
		{
			IList<CalibrationItem> result = new List<CalibrationItem>();
			if (File.Exists(calibrationFilePath))
			{
				result = (IList<CalibrationItem>)_serializationService.Deserialize(calibrationFilePath);
			}
			return result;
		}

		private IList<PolarCoordinate> _polarCoordinatesF;
		public IList<PolarCoordinate> PolarCoordinatesF
		{
			get
			{
				_polarCoordinatesF = new List<PolarCoordinate>();

				LineSegment2D baseLine = new LineSegment2D(FCenterPoint, FPoint1);
				foreach (Point eyePoint in CalibrationList.Select(item => item.EyePoint))
				{
					double angle = 360 - MathHelper.AngleBetweenLines(baseLine, new LineSegment2D(FCenterPoint, eyePoint));
					_polarCoordinatesF.Add(new PolarCoordinate()
					{
						Radius = new LineSegment2D(FCenterPoint, eyePoint).Length,
						Angle = angle,
					});
				}
				return _polarCoordinatesF;
			}
		}

		private IList<PolarCoordinate> _polarCoordinatesS;
		public IList<PolarCoordinate> PolarCoordinatesS
		{
			get
			{
				_polarCoordinatesS = new List<PolarCoordinate>();

				LineSegment2D baseLine = new LineSegment2D(SCenterPoint, SPoint1);
				foreach (Point sPoint in ScreenCalibrationPoints)
				{
					double angle = MathHelper.AngleBetweenLines(baseLine, new LineSegment2D(SCenterPoint, sPoint));
					_polarCoordinatesS.Add(new PolarCoordinate()
					{
						Radius = new LineSegment2D(FCenterPoint, sPoint).Length,
						Angle = angle,
					});
				}
				return _polarCoordinatesS;
			}
		}
	}
}
