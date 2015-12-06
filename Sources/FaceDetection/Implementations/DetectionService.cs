using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Interfaces;
using FaceDetection.Models;

namespace FaceDetection
{
	public class DetectionService : IDetectionService
	{
		private const string faceHaarCascadePath = @"Cascades\haarcascade_frontalface_alt.xml";
		private const string eyeHaarCascadePath = @"Cascades\haarcascade_eye_treeglasses.xml";
		private const string mouthHaarCascadePath = @"Cascades\haarcascade_mouth.xml";
		private const string noseHaarCascadePath = @"Cascades\haarcascade_nose.xml";

		private CascadeClassifier _eyeCascadeClassifier;
		private CascadeClassifier _mouthCascadeClassifier;
		private CascadeClassifier _noseCascadeClassifier;
		private CascadeClassifier _faceCascadeClassifier;

		public DetectionService()
		{
			_eyeCascadeClassifier = new CascadeClassifier(eyeHaarCascadePath);
			_mouthCascadeClassifier = new CascadeClassifier(mouthHaarCascadePath);
			_noseCascadeClassifier = new CascadeClassifier(noseHaarCascadePath);
			_faceCascadeClassifier = new CascadeClassifier(faceHaarCascadePath);
		}

		public DetectionResult GetDetectionResult(Image<Bgr, Byte> imageFrame)
		{
			DetectionResult result = new DetectionResult() { Status = DetectionStatus.Success };
			try
			{
				Rectangle face = GetFace(imageFrame);
				Image<Bgr, Byte> faceFrame = imageFrame.Copy(face);
				IList<Rectangle> detectedEyes = GetEyes(faceFrame, face);
				EyePair eyeEdgesPair = GetEyeEdgesPair(detectedEyes);
				EyePair eyeCentersPair = GetEyeCentersPair(detectedEyes.Select(eye => eye.Center()).ToList());
				Point mouthCenterPoint = GetMouth(faceFrame, face);
				Point nosePoint = GetNose(faceFrame, eyeEdgesPair, face);
				Point bridgeNosePoint = MathHelper.GetPerpendicularPoint(eyeEdgesPair.LeftEye, eyeEdgesPair.RightEye, nosePoint);

				result.Face = face;
				result.DetectedEyes = detectedEyes;
				result.EyeEdgesPair = eyeEdgesPair;
				result.EyeCentersPair = eyeCentersPair;
				result.MouthCenterPoint = mouthCenterPoint;
				result.NosePoint = nosePoint;
				result.BridgeNosePoint = bridgeNosePoint;
			}
			catch (Exception)
			{
				result.Status = DetectionStatus.Error;
				MessageBox.Show("Unable to find face");
			}
			return result;
		}

		public Rectangle GetFace(Image<Bgr, Byte> imageFrame)
		{
			return _faceCascadeClassifier.DetectMultiScale(imageFrame, 1.1, 1, new Size(250, 250)).First();
		}

		public Point GetMouth(Image<Bgr, Byte> faceFrame, Rectangle face)
		{
			Color color = Color.Red;
			int mouthFrameHeight = Convert.ToInt32(faceFrame.Height * 0.3);
			int mouthFrameY = Convert.ToInt32(faceFrame.Height * 0.7);
			Rectangle mouthFrame = new Rectangle(0, mouthFrameY, faceFrame.Width, mouthFrameHeight);
			faceFrame.Draw(mouthFrame, new Bgr(color), 4);


			Image<Bgr, byte> mouthRegion = faceFrame.Copy(mouthFrame);
			IEnumerable<Rectangle> mouths = _mouthCascadeClassifier.DetectMultiScale(mouthRegion, 1.1, 1, Size.Empty);
			IList<Rectangle> mouthCandidates = new List<Rectangle>();
			foreach (Rectangle mouth in mouths)
			{
				Rectangle mouthAbsolute = new Rectangle(mouth.X + face.X, mouth.Y + mouthFrameY + face.Y, mouth.Width, mouth.Height);
				mouthCandidates.Add(mouthAbsolute);
			}
			Rectangle detectedMouht = mouthCandidates.First();
			return detectedMouht.Center();
		}

		public Point GetNose(Image<Bgr, Byte> faceFrame, EyePair eyePair, Rectangle face)
		{
			Color color = Color.Brown;
			int noseFrameHeight = Convert.ToInt32(faceFrame.Height * 0.25);
			int noseFrameY = Convert.ToInt32(faceFrame.Height * 0.5);
			Rectangle noseFrame = new Rectangle(0, noseFrameY, faceFrame.Width, noseFrameHeight);
			faceFrame.Draw(noseFrame, new Bgr(color), 4);


			Image<Bgr, byte> noseRegion = faceFrame.Copy(noseFrame);
			IEnumerable<Rectangle> noses = _noseCascadeClassifier.DetectMultiScale(noseRegion, 1.1, 2, Size.Empty);
			IList<Rectangle> noseCandidates = new List<Rectangle>();
			foreach (Rectangle nose in noses)
			{
				Rectangle noseAbsolute = new Rectangle(nose.X + face.X, nose.Y + noseFrameY + face.Y, nose.Width, nose.Height);
				Point noseCenter = noseAbsolute.Center();
				if (NoseIsBeetweenTheEyes(noseAbsolute, noseCenter, eyePair))
				{
					noseCandidates.Add(noseAbsolute);
				}
			}
			Rectangle detectedNose = GetNoseWithMaxSquare(noseCandidates);
			return detectedNose.Center();
		}

		public IList<Rectangle> GetEyes(Image<Bgr, Byte> faceFrame, Rectangle face)
		{
			Color color = Color.Aqua;
			Rectangle eyesFrame = new Rectangle(0, 0, faceFrame.Width, Convert.ToInt32(faceFrame.Height * 0.7));
			faceFrame.Draw(eyesFrame, new Bgr(color), 4);

			Image<Bgr, byte> eyesRegion = faceFrame.Copy(eyesFrame);

			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			IList<Rectangle> eyesDetected = new List<Rectangle>();
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X + face.X, eye.Y + face.Y, eye.Width, eye.Height);
				eyesDetected.Add(eyeAbsolute);
			}
			return eyesDetected;
		}


		private bool NoseIsBeetweenTheEyes(Rectangle noseAbsolute, Point noseCenter, EyePair eyePair)
		{
			return noseCenter.X >= eyePair.LeftEye.X && noseCenter.X <= eyePair.RightEye.X
				&& noseAbsolute.X >= eyePair.LeftEye.X && noseAbsolute.X <= eyePair.RightEye.X;
		}

		private Rectangle GetNoseWithMaxSquare(IList<Rectangle> noseCandidates)
		{
			int maxSquare = noseCandidates.Max(n => n.Square());
			return noseCandidates.First(n => n.Square() == maxSquare);
		}

		private EyePair GetEyeCentersPair(IList<Point> eyesCenters)
		{
			EyePair result = new EyePair();
			if (eyesCenters[0].X < eyesCenters[1].X)
			{
				result.LeftEye = eyesCenters[0];
				result.RightEye = eyesCenters[1];
			}
			else
			{
				result.LeftEye = eyesCenters[1];
				result.RightEye = eyesCenters[0];
			}
			return result;
		}

		private EyePair GetEyeEdgesPair(IList<Rectangle> eyesRectangles)
		{
			EyePair result = new EyePair();
			if (eyesRectangles[0].X < eyesRectangles[1].X)
			{
				result.LeftEye = new Point(eyesRectangles[0].X, eyesRectangles[0].Y + eyesRectangles[0].Height / 2);
				result.RightEye = new Point(eyesRectangles[1].X + eyesRectangles[1].Width, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
			}
			else
			{
				result.LeftEye = new Point(eyesRectangles[1].X, eyesRectangles[1].Y + eyesRectangles[1].Height / 2);
				result.RightEye = new Point(eyesRectangles[0].X + eyesRectangles[0].Width, eyesRectangles[0].Y + eyesRectangles[0].Height / 2);
			}
			return result;
		}
	}
}
