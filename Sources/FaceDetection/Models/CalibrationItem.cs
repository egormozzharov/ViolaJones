using System;
using System.Drawing;

namespace FaceDetection.Models
{
	[Serializable]
	public class CalibrationItem
	{
		public Point RightEyePoint { get; set; }

		public Point LeftEyePoint { get; set; }

		public Point ScreenPoint { get; set; }

		public Point BridgeCoordinatesAbsolute { get; set; }
	}
}
