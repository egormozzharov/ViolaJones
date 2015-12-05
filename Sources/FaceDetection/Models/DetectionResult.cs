using System.Collections.Generic;
using System.Drawing;

namespace FaceDetection.Models
{
	public class DetectionResult
	{
		public Rectangle Face { get; set; }

		public IList<Rectangle> DetectedEyes { get; set; }

		public EyePair EyeEdgesPair { get; set; }

		public EyePair EyeCentersPair { get; set; }

		public Point MouthCenterPoint { get; set; }

		public Point NosePoint { get; set; }

		public DetectionStatus Status { get; set; }
	}
}
