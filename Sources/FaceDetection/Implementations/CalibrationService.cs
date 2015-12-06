using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Emgu.CV.Structure;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	public class CalibrationService
	{
		private readonly SerializationService _serializationService;

		public CalibrationService(string calibrationFileName)
		{
			_serializationService = new SerializationService();
			CalibrationInfoList = TryLoadCalibrationList(calibrationFileName);
		}

		public void SaveCalibrationList(string calibrationFileName)
		{
			_serializationService.Serialize(CalibrationInfoList, calibrationFileName);
		}

		private IList<CalibrationItem> TryLoadCalibrationList(string calibrationFilePath)
		{
			IList<CalibrationItem> result = new List<CalibrationItem>();
			if (File.Exists(calibrationFilePath))
			{
				result = (IList<CalibrationItem>)_serializationService.Deserialize(calibrationFilePath);
			}
			return result;
		}

		public IList<CalibrationItem> CalibrationInfoList { get; set; }


		public LineSegment2D FBaseLine
		{
			get { return new LineSegment2D(FCenterPoint, FPoint1); }
		}

		public Point FCenterPoint
		{
			get { return CalibrationInfoList.Last().EyePoint; }
		}

		public Point FPoint1
		{
			get { return CalibrationInfoList.First().EyePoint; }
		}


		public LineSegment2D SBaseLine
		{
			get { return new LineSegment2D(SCenterPoint, SPoint1); }
		}

		public Point SCenterPoint
		{
			get { return CalibrationInfoList.Last().ScreenPoint; }
		}

		public Point SPoint1
		{
			get { return CalibrationInfoList.First().ScreenPoint; }
		}

		private IList<PolarCoordinatePair> _fPolarCoordinatesWithCorrespondingSCoordinate;
		public IList<PolarCoordinatePair> FPolarCoordinatesWithCorrespondingSCoordinate
		{
			get
			{
				_fPolarCoordinatesWithCorrespondingSCoordinate = new List<PolarCoordinatePair>();
				foreach (CalibrationItem item in CalibrationInfoList)
				{
					Point fPoint = item.EyePoint;
					Point sPoint = item.ScreenPoint;
					_fPolarCoordinatesWithCorrespondingSCoordinate.Add(new PolarCoordinatePair()
					{
						FPolarCoordinate = new PolarCoordinate()
						{
							Radius = new LineSegment2D(FCenterPoint, fPoint).Length,
							Angle = GetFLineAngle(fPoint),
						},
						SPolarCoordinate = new PolarCoordinate()
						{
							Radius = new LineSegment2D(SCenterPoint, sPoint).Length,
							Angle = GetSLineAngle(sPoint),
						}
					});
				}
				return _fPolarCoordinatesWithCorrespondingSCoordinate;
			}
		}

		public IList<PolarCoordinate> FPolarCoordinates
		{
			get { return FPolarCoordinatesWithCorrespondingSCoordinate.Select(pair => pair.FPolarCoordinate).ToList(); }
		} 

		public IList<PolarCoordinate> SPolarCoordinates
		{
			get { return FPolarCoordinatesWithCorrespondingSCoordinate.Select(pair => pair.SPolarCoordinate).ToList(); }
		}

		public double GetFLineAngle(Point fPoint)
		{
			double angle = MathHelper.AngleBetweenLines(FBaseLine, new LineSegment2D(FCenterPoint, fPoint));
			if (angle != 0)
			{
				angle = 360 - angle;
			}
			return angle;
		}

		public double GetSLineAngle(Point sPoint)
		{
			return MathHelper.AngleBetweenLines(SBaseLine, new LineSegment2D(SCenterPoint, sPoint));
		}

		public PolarCoordinate GetClosestLessFPolarCoordinate(PolarCoordinate polarCoordinate)
		{
			double minAngle = FPolarCoordinates.Min(f => f.Angle);
			PolarCoordinate closestCoordinate = FPolarCoordinates.First(p => p.Angle == minAngle);
			foreach (PolarCoordinate fPoint in FPolarCoordinates)
			{
				if ((fPoint.Angle <= polarCoordinate.Angle)  && (fPoint.Angle > closestCoordinate.Angle))
				{
					closestCoordinate = fPoint;
				}
			}
			return closestCoordinate;
		}

		public PolarCoordinate GetClosestBiggerFPolarCoordinate(PolarCoordinate polarCoordinate)
		{
			double maxAngle = FPolarCoordinates.Max(f => f.Angle);
			PolarCoordinate closestCoordinate = FPolarCoordinates.First(p => p.Angle == maxAngle);
			foreach (PolarCoordinate fPoint in FPolarCoordinates)
			{
				if ((fPoint.Angle >= polarCoordinate.Angle) && (fPoint.Angle < closestCoordinate.Angle))
				{
					closestCoordinate = fPoint;
				}
			}
			return closestCoordinate;
		}

		public double GetScreenAngle(PolarCoordinate eyeDetectedCoordinate)
		{
			PolarCoordinate FfLessCoordinate = GetClosestLessFPolarCoordinate(eyeDetectedCoordinate);
			PolarCoordinate FfBigCoordinate = GetClosestBiggerFPolarCoordinate(eyeDetectedCoordinate);

			double Ff = eyeDetectedCoordinate.Angle;
			double FfLess = FfLessCoordinate.Angle;
			double FfBig = FfBigCoordinate.Angle;
			double FsLess = FPolarCoordinatesWithCorrespondingSCoordinate.First(pair =>
					(pair.FPolarCoordinate.Angle == FfLessCoordinate.Angle) &&
					(pair.FPolarCoordinate.Radius == FfLessCoordinate.Radius)).SPolarCoordinate.Angle;
			double FsBig = FPolarCoordinatesWithCorrespondingSCoordinate.First(pair =>
				(pair.FPolarCoordinate.Angle == FfBigCoordinate.Angle && pair.FPolarCoordinate.Radius == FfBigCoordinate.Radius)).SPolarCoordinate.Angle;
			//if detected point is the calibration point`
			if (FfLess == FfBig)
			{
				return FsLess;
			}
			double result = ((Ff - FfLess)/(FfBig - FfLess))*(FsBig - FsLess) + FsBig;
			return result;
		}

		/// <summary>
		/// Recalculate calibrated coordinates based on NoseBridge shifting.
		/// </summary>
		public void RecalculateCalibration()
		{
			//CalibrationItem centerCalibrationItem = CalibrationInfoList.Last();
			//Point centerNoseBridgeAbsolute = centerCalibrationItem.BridgeCoordinatesAbsolute;
			//foreach (CalibrationItem item in CalibrationInfoList)
			//{
			//	Point bridgeNoseAbsolute = item.BridgeCoordinatesAbsolute;
			//	Point delta = new Point(bridgeNoseAbsolute.X - centerNoseBridgeAbsolute.X, bridgeNoseAbsolute.Y - centerNoseBridgeAbsolute.Y);


			//}
		}
	}
}
