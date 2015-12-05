using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	public class CalibrationService
	{
		private readonly SerializationService _serializationService;
		private int _currentSceenCalibrationPoint = 0;

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
				return ScreenCalibrationPoints[_currentSceenCalibrationPoint.ToString()];
			}
		}

		public void SaveCalibrationList(string calibrationFileName)
		{
			_serializationService.Serialize(CalibrationList, calibrationFileName);
		}

		public Point MoveScreenCalibrationPoint()
		{
			_currentSceenCalibrationPoint++;
			string currentKey = _currentSceenCalibrationPoint.ToString();
			return ScreenCalibrationPoints[currentKey];
		}

		public bool HasNextCalibrationPoint
		{
			get { return IsCalibrated; }
		}

		public bool IsCalibrated
		{
			get { return CalibrationList.Count == 9; }
		}

		private static Rectangle Screen
		{
			get
			{
				return System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			}
		}

		private IDictionary<string, Point> ScreenCalibrationPoints
		{
			get
			{
				IDictionary<string, Point> sPoints = new Dictionary<string, Point>();
				sPoints.Add("1", new Point((Screen.X + Screen.Width) - PaddingWidth, (Screen.Y + Screen.Height / 2)));
				sPoints.Add("2", new Point((Screen.X + Screen.Width) - PaddingWidth, Screen.Y));
				sPoints.Add("3", new Point((Screen.X + Screen.Width / 2), Screen.Y));
				sPoints.Add("4", new Point(Screen.X, Screen.Y));
				sPoints.Add("5", new Point(Screen.X, Screen.Y + Screen.Height / 2));
				sPoints.Add("6", new Point(Screen.X, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add("7", new Point(Screen.X + Screen.Width / 2, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add("8", new Point((Screen.X + Screen.Width) - PaddingWidth, (Screen.Y + Screen.Height) - PaddingHeight * 2));
				sPoints.Add("9", new Point(Screen.X + Screen.Width / 2, Screen.Y + Screen.Height / 2));
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
	}
}
