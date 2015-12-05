using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FaceDetection.Models;

namespace FaceDetection.Helpers
{
	public static class DrawingHelper
	{
		public static void DrawDetectedObject(Image<Bgr, Byte> faceFrame, Rectangle detectedObject, Color color)
		{
			faceFrame.Draw(detectedObject, new Bgr(color));
			DrawPoint(detectedObject.Center(), faceFrame, color);
		}

		public static void DrawPoint(Point centerPoint, Image<Bgr, Byte> imageFrame, Color color)
		{
			imageFrame.Draw(new CircleF(centerPoint, 2), new Bgr(color));
			//DrawCoordinates(centerPoint, imageFrame);
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
	}
}
