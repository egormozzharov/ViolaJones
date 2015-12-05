using System;
using System.Drawing;

namespace FaceDetection.Models
{
	[Serializable]
	public class CalibrationItem
	{
		public Point EyePoint { get; set; }

		public Point ScreenPoint { get; set; }

		public Point FaceLeftHighPointAbsolute { get; set; }

		public Point BridgeCoordinatesRelative { get; set; }
	}
}
