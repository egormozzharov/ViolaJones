using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FaceDetection.Implementations
{
	public class SerializationService
	{
		public void Serialize(object obj, string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, obj);
			stream.Close();
		}

		public object Deserialize(string filePath)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			object obj = formatter.Deserialize(stream);
			stream.Close();
			return obj;
		}
	}
}
