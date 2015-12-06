using System;
using System.Drawing;

namespace FaceDetection.Models
{
	[Serializable]
	public class CalibrationItem
	{
		public Point EyePoint { get; set; }

		public Point ScreenPoint { get; set; }

		public Point FaceLeftHighPointAbsolute { private get; set; }

		public Point BridgeCoordinatesRelative { private get; set; }

		public Point BridgeCoordinatesAbsolute
		{
			get
			{
				return new Point(FaceLeftHighPointAbsolute.X + BridgeCoordinatesRelative.X, FaceLeftHighPointAbsolute.Y + BridgeCoordinatesRelative.Y);
			}
		}
	}
}
