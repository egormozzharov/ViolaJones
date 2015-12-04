using System;
using System.Collections.Generic;
using FaceDetection.Interfaces;
using FaceDetection.Models;

namespace FaceDetection.Implementations
{
	class CalibrationService : ICalibrationService
	{
		public IList<CalibrationItem> CalibrationList { get; set; } 

		public bool CalibrationListIsEmpty()
		{
			throw new NotImplementedException();
		}

		private void TryLoadCalibrationList()
		{
			throw new NotImplementedException();
		}

		private void GetScreenParams()
		{
			//
		}
	}
}
