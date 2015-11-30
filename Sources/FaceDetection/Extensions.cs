using System.Drawing;

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
	}
}
