using System;
using System.Drawing;

namespace FaceDetection
{
	public static class MathHelper
	{
		public static double AngleWithHorizont(Point p1, Point p2)
		{
			return RadianToDegree(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X)) % 180;
		}

		private static double RadianToDegree(double radian)
		{
			var degree = radian * (180.0 / Math.PI);
			if (degree < 0)
				degree = 360 + degree;

			return degree;
		}

		public static double LineLength(Point start, Point end)
		{
			return Math.Sqrt(Math.Pow((end.Y - start.Y), 2) + Math.Pow((end.X - start.X), 2));
		}
	}
}
