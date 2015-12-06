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
	public static class DrawingHelper
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

		public static System.Drawing.Image FixedSize(Image image, int Width, int Height, bool needToFill)
		{
			#region много арифметики

			int sourceWidth = image.Width;
			int sourceHeight = image.Height;
			int sourceX = 0;
			int sourceY = 0;
			double destX = 0;
			double destY = 0;

			double nScale = 0;
			double nScaleW = 0;
			double nScaleH = 0;

			nScaleW = ((double) Width/(double) sourceWidth);
			nScaleH = ((double) Height/(double) sourceHeight);
			if (!needToFill)
			{
				nScale = Math.Min(nScaleH, nScaleW);
			}
			else
			{
				nScale = Math.Max(nScaleH, nScaleW);
				destY = (Height - sourceHeight*nScale)/2;
				destX = (Width - sourceWidth*nScale)/2;
			}

			if (nScale > 1)
				nScale = 1;

			int destWidth = (int) Math.Round(sourceWidth*nScale);
			int destHeight = (int) Math.Round(sourceHeight*nScale);

			#endregion

			System.Drawing.Bitmap bmPhoto = null;
			try
			{
				bmPhoto = new System.Drawing.Bitmap(destWidth + (int) Math.Round(2*destX), destHeight + (int) Math.Round(2*destY));
			}
			catch (Exception ex)
			{
				throw new ApplicationException(
					string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
						destWidth, destX, destHeight, destY, Width, Height), ex);
			}
			using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
			{
				//grPhoto.InterpolationMode = I;
				//grPhoto.CompositingQuality = _compositingQuality;
				//grPhoto.SmoothingMode = _smoothingMode;

				Rectangle to = new System.Drawing.Rectangle((int) Math.Round(destX), (int) Math.Round(destY), destWidth, destHeight);
				Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
				//Console.WriteLine("From: " + from.ToString());
				//Console.WriteLine("To: " + to.ToString());
				grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

				return bmPhoto;
			}
		}

		public static void DrawLine(Control control, Point pointA, Point pointB)
		{
			Pen pen = new Pen(Color.Red);
			control.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
			{
				Graphics g = e.Graphics;
				g.DrawLine(pen, pointA, pointB);
			});
			control.Refresh();
		}

		public static void DrawPoint(Control control, Point point)
		{
			Pen pen = new Pen(Color.Red);
			control.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.DarkRed), point.X, point.Y, 4, 4);
			});
			control.Refresh();
		}
	}
}
