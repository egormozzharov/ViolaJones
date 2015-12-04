using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace FaceDetection
{
	public static class FaceRotation
	{
		public static double GetRotationAroundVertical(Point e, Point d, LineSegment2D lineBetweenEyes)
		{
			LineSegment2D EDSegment = new LineSegment2D(e, d);
			double radians = -(EDSegment.Length / lineBetweenEyes.Length / 0.55);
			double result = MathHelper.RadianToDegree(radians);
			return Math.Abs(180 - result);
		}

		public static double GetSideLongTilt(EyePair eyePair)
		{
			return MathHelper.AngleWithHorizont(eyePair.LeftEye, eyePair.RightEye);
		}

		public static double GetBackForthTilt(Point mouthCenterPoint, Point perpendicularPointFromNose, LineSegment2D lineBetweenEyes)
		{
			double result;
			LineSegment2D FD = new LineSegment2D(mouthCenterPoint, perpendicularPointFromNose);
			LineSegment2D AB = lineBetweenEyes;
			double r = Math.Abs(FD.Length) / Math.Abs(AB.Length);
			double expression = (r * Math.Cos(-0.24)) / 0.92;
			result = Math.Acos(expression) - 0.24;
			double degrees = MathHelper.RadianToDegree(result);
			return degrees;
		}
	}
}
