using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FaceDetection.Models;

namespace FaceDetection.Helpers
{
	public class DrawingHelper
	{
		public static void DrawDetectedObject(Image<Bgr, Byte> faceFrame, Rectangle detectedObject, Color color)
		{
			faceFrame.Draw(detectedObject, new Bgr(color));
			DrawPoint(detectedObject.Center(), faceFrame, color);
		}

		public static void DrawPoint(Point centerPoint, Image<Bgr, Byte> imageFrame, Color color)
		{
			imageFrame.Draw(new CircleF(centerPoint, 1), new Bgr(color));
		}

		public static void DrawPoints(IEnumerable<Point> centerPoints, Image<Bgr, Byte> imageFrame, Color color)
		{
			foreach (Point point in centerPoints)
			{
				DrawPoint(point, imageFrame, color);
			}
		}

		public static void DrawCoordinates(Point centerPoint, Image<Bgr, Byte> imageFrame)
		{
			imageFrame.Draw(String.Format("x:{0}, y:{1}", centerPoint.X, centerPoint.Y), centerPoint,
				FontFace.HersheyComplex, 0.3, new Bgr(Color.Black));
		}

		public static void DrawLines(IEnumerable<LineSegment2D> lines, Image<Bgr, Byte> frame)
		{
			foreach (LineSegment2D line in lines)
			{
				frame.Draw(line, new Bgr(Color.Black), 1);
			}
		}

		public static void DrawLine(LineSegment2D line, Image<Bgr, Byte> frame, Color color)
		{
			frame.Draw(line, new Bgr(color), 1);
		}

		public static void DrawDetectedObjects(Image<Bgr, Byte> frame, DetectionResult detectionResult)
		{
			detectionResult.DetectedEyes.ToList().ForEach(eye =>
			{
				DrawDetectedObject(frame, eye, Color.Blue);
			});
			DrawPoint(detectionResult.MouthCenterPoint, frame, Color.Red);
			DrawPoint(detectionResult.NosePoint, frame, Color.Brown);
		}

		public static Image<Bgr, Byte> GetImageFrame(Image<Bgr, Byte> imageFrame, Rectangle detectedFace)
		{
			return imageFrame.Copy(detectedFace);
		}

		public static void DrawLine(Control control, Point pointA, Point pointB, Color color)
		{
			Pen pen = new Pen(color);
			control.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
			{
				Graphics g = e.Graphics;
				g.DrawLine(pen, pointA, pointB);
			});
			control.Refresh();
		}

		public static void DrawPoint(Control control, Point point, Color color, int width = 3)
		{
			control.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
			{
				e.Graphics.FillRectangle(new SolidBrush(color), point.X, point.Y, width, width);
			});
			control.Refresh();
		}

		public static void DrawPoint(Image image, Point point, Color color, int width = 3)
		{
			Graphics g = Graphics.FromImage(image);
			g.FillRectangle(new SolidBrush(color), point.X, point.Y, width, width);
		}

		public static void Clear(Control control)
		{
			control.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
			{
				e.Graphics.Clear(Color.White);
			});
			control.Refresh();
		}
	}
}
