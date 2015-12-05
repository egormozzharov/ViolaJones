using System;
using System.Collections.Generic;
using System.Drawing;
using FaceDetection.Interfaces;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	public class CalibrationService : ICalibrationService
	{
		public IList<CalibrationItem> CalibrationList { get; set; } 

		public bool CalibrationListIsEmpty()
		{
			throw new NotImplementedException();
		}

		private void TryLoadCalibrationList()
		{
			throw new NotImplementedException();
		}

		private static Rectangle Screen
		{
			get
			{
				return System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			}
		}

		private IDictionary<string, Point> GetScreenCalibrationPoints()
		{
			IDictionary<string, Point> sPoints = new Dictionary<string, Point>();
			sPoints.Add("1", new Point(Screen.X + Screen.Width, Screen.Y + Screen.Height/2));
			sPoints.Add("2", new Point(Screen.X + Screen.Width, Screen.Y));
			sPoints.Add("3", new Point(Screen.X + Screen.Width/2, Screen.Y));
			sPoints.Add("4", new Point(Screen.X, Screen.Y));
			sPoints.Add("5", new Point(Screen.X, Screen.Y + Screen.Height/2));
			sPoints.Add("6", new Point(Screen.X, Screen.Y + Screen.Height));
			sPoints.Add("7", new Point(Screen.X + Screen.Width/2, Screen.Y + Screen.Height));
			sPoints.Add("8", new Point(Screen.X + Screen.Width, Screen.Y + Screen.Height));
			sPoints.Add("center", new Point(Screen.X + Screen.Width/2, Screen.Y + Screen.Height/2));
			return sPoints;
		} 
	}
}
