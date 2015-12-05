using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Interfaces;

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

		public Rectangle GetFace(Image<Bgr, Byte> imageFrame)
		{
			return _faceCascadeClassifier.DetectMultiScale(imageFrame, 1.1, 1, new Size(250, 250)).First();
		}

		public Point GetMouth(Image<Bgr, Byte> faceFrame)
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
				Rectangle mouthAbsolute = new Rectangle(mouth.X, mouth.Y + mouthFrameY, mouth.Width, mouth.Height);
				mouthCandidates.Add(mouthAbsolute);
			}
			Rectangle detectedMouht = mouthCandidates.First();
			return detectedMouht.Center();
		}

		public Point GetNose(Image<Bgr, Byte> faceFrame, EyePair eyePair)
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
				Rectangle noseAbsolute = new Rectangle(nose.X, nose.Y + noseFrameY, nose.Width, nose.Height);
				Point noseCenter = noseAbsolute.Center();
				if (NoseIsBeetweenTheEyes(noseAbsolute, noseCenter, eyePair))
				{
					noseCandidates.Add(noseAbsolute);
				}
			}
			Rectangle detectedNose = GetNoseWithMaxSquare(noseCandidates);
			return detectedNose.Center();
		}

		public IList<Rectangle> GetEyes(Image<Bgr, Byte> faceFrame)
		{
			Color color = Color.Aqua;
			Rectangle eyesFrame = new Rectangle(0, 0, faceFrame.Width, Convert.ToInt32(faceFrame.Height * 0.7));
			faceFrame.Draw(eyesFrame, new Bgr(color), 4);

			Image<Bgr, byte> eyesRegion = faceFrame.Copy(eyesFrame);

			IEnumerable<Rectangle> eyes = _eyeCascadeClassifier.DetectMultiScale(eyesRegion, 1.1, 2, Size.Empty);
			IList<Rectangle> eyesDetected = new List<Rectangle>();
			foreach (Rectangle eye in eyes)
			{
				Rectangle eyeAbsolute = new Rectangle(eye.X, eye.Y, eye.Width, eye.Height);
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
	}
}
