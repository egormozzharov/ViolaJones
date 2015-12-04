using System;
using System.Drawing;

namespace FaceDetection
{
	public static class MathHelper
	{
		public static double AngleWithHorizont(Point p1, Point p2)
		{
			return RadianToDegree(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X));
		}

		public static double RadianToDegree(double radian)
		{
			var degree = radian * (180.0 / Math.PI);
			if (degree < 0)
				degree = 360 + degree;

			return degree % 180;
		}

		public static double LineLength(Point start, Point end)
		{
			return Math.Sqrt(Math.Pow((end.Y - start.Y), 2) + Math.Pow((end.X - start.X), 2));
		}

		public static Point GetPerpendicularPoint(Point p1, Point p2, Point p3)
		{
			int y1 = p1.Y;
			int x1 = p1.X;
			int y2 = p2.Y;
			int x2 = p2.X;
			int y3 = p3.Y;
			int x3 = p3.X;
			double k = ((y2 - y1) * (x3 - x1) - (x2 - x1) * (y3 - y1)) / (Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
			int x4 = x3 - Convert.ToInt32(k * (y2 - y1));
			int y4 = y3 + Convert.ToInt32(k * (x2 - x1));
			Point p4 = new Point(x4, y4);
			return p4;
		}
	}
}
