using System;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Models;

namespace FaceDetection.Interfaces
{
	public interface IDetectionService
	{
		DetectionResult GetDetectionResult(Image<Bgr, Byte> imageFrame);
	}
}
