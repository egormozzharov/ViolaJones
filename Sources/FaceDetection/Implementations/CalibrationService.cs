using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV.Structure;
using FaceDetection.Helpers;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	public class CalibrationService
	{
		private readonly SerializationService _serializationService;

		public CalibrationService()
		{
			_serializationService = new SerializationService();
			CalibrationInfoList = GetCalibrationList();
		}

		public void SaveCalibrationList(string calibrationFileName)
		{
			_serializationService.Serialize(CalibrationInfoList, calibrationFileName);
		}

		private IList<CalibrationItem> GetCalibrationList()
		{
			Point bridge = new Point(345, 240);
			IList<CalibrationItem> result = new List<CalibrationItem>();
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(399, 235),
				LeftEyePoint = new Point(290, 233),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[0],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(399, 232),
				LeftEyePoint = new Point(288, 230),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[1],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(407, 234),
				LeftEyePoint = new Point(296, 232),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[2],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(417, 235),
				LeftEyePoint = new Point(305, 234),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[3],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(416, 240),
				LeftEyePoint = new Point(304, 239),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[4],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(415, 242),
				LeftEyePoint = new Point(303, 241),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[5],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(406, 242),
				LeftEyePoint = new Point(295, 241),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[6],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(401, 242),
				LeftEyePoint = new Point(291, 241),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[7],
			});
			result.Add(new CalibrationItem()
			{
				BridgeCoordinatesAbsolute = bridge,
				RightEyePoint = new Point(410, 240),
				LeftEyePoint = new Point(298, 240),
				ScreenPoint = ScreenCalibrationHelper.ScreenCalibrationPoints[8],
			});
			return result;
		}

		public IList<CalibrationItem> CalibrationInfoList { get; set; }


		public LineSegment2D FBaseLine
		{
			get { return new LineSegment2D(FCenterPoint, FPoint1); }
		}

		public Point FCenterPoint
		{
			get { return CalibrationInfoList.Last().RightEyePoint; }
		}

		public Point FPoint1
		{
			get { return CalibrationInfoList.First().RightEyePoint; }
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
					Point fPoint = item.RightEyePoint;
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
			double result = ((Ff - FfLess)/(FfBig - FfLess))*(FsBig - FsLess) + FsLess;
			return result;
		}

		/// <summary>
		/// Recalculate calibrated coordinates based on NoseBridge shifting.
		/// </summary>
		public void RecalculateCalibration()
		{
			foreach (CalibrationItem item in CalibrationInfoList)
			{
				Point eyePoint = item.RightEyePoint;
				item.RightEyePoint = RecalculateCalibratedPoint(item.BridgeCoordinatesAbsolute, eyePoint);
			}
		}

		public Point RecalculateCalibratedPoint(Point bridgeNose, Point eyePoint)
		{
			CalibrationItem centerCalibrationItem = CalibrationInfoList.Last();
			Point centerNoseBridge = centerCalibrationItem.BridgeCoordinatesAbsolute;
			Point delta = new Point(bridgeNose.X - centerNoseBridge.X, bridgeNose.Y - centerNoseBridge.Y);
			eyePoint = new Point(eyePoint.X - delta.X, eyePoint.Y - delta.Y);
			return eyePoint;
		}
	}
}
