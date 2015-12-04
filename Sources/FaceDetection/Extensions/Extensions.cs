using System.Drawing;
using Emgu.CV.Structure;

namespace FaceDetection
{
	public static class Extensions
	{
		public static Point Center(this Rectangle rect)
		{
			return new Point(rect.Left + rect.Width / 2,
				rect.Top + rect.Height / 2);
		}

		public static int Square(this Rectangle rect)
		{
			return rect.Height*rect.Width;
		}

		public static Point MidPoint(this LineSegment2D line)
		{
			int xA = line.P1.X;
			int xB = line.P2.X;
			int yA = line.P1.Y;
			int yB = line.P2.Y;
			int xResult = (xA + xB)/2;
			int yResult = (yA + yB)/2;
			return new Point(xResult, yResult);
		}
	}
}
