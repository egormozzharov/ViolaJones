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

		public static void DrawLines(IEnumerable<LineSegment2D> lines, Image<Bgr, Byte> faceFrame)
		{
			foreach (LineSegment2D line in lines)
			{
				faceFrame.Draw(line, new Bgr(Color.Black), 1);
			}
		}

		public static void DrawDetectedObjects(Image<Bgr, Byte> faceFrame, DetectionResult detectionResult)
		{
			detectionResult.DetectedEyes.ToList().ForEach(eye =>
			{
				DrawDetectedObject(faceFrame, eye, Color.Blue);
			});
			DrawPoint(detectionResult.MouthCenterPoint, faceFrame, Color.Red);
			DrawPoint(detectionResult.NosePoint, faceFrame, Color.Brown);
		}
	}
}
