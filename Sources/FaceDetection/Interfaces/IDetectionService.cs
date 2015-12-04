using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Interfaces
{
	public interface IDetectionService
	{
		Rectangle GetFace(Image<Bgr, Byte> imageFrame);

		Point GetMouth(Image<Bgr, Byte> faceFrame);

		Point GetNose(Image<Bgr, Byte> faceFrame, EyePair eyePair);

		IList<Rectangle> GetEyes(Image<Bgr, Byte> faceFrame);
	}
}
