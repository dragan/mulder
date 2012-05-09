using System;
using System.IO;

namespace Mulder.Base.IO
{
	public class FileSystem : IFileSystem
	{
		public const int BufferSize = 32768;
		
		public FileSystem()
		{
		}
		
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public void CreateDirectory(string path)
		{
			if (String.IsNullOrEmpty(path))
				return;
			
			if (Directory.Exists(path))
				return;
			
			Directory.CreateDirectory(path);
		}

		public void ChangeDirectory(string path, Action action)
		{
			var originalDirectory = Directory.GetCurrentDirectory();
			
			Directory.SetCurrentDirectory(path);
			
			action();
			
			Directory.SetCurrentDirectory(originalDirectory);
		}

		public void WriteStreamToFile(string filename, Stream stream)
		{
			CreateDirectory(Path.GetDirectoryName(filename));
			
			var fileSize = 0;
			using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
				int bytesRead;
				var buffer = new byte[BufferSize];
				
				do {
					bytesRead = stream.Read(buffer, 0, buffer.Length);
					fileSize += bytesRead;

					if (bytesRead > 0) {
						fileStream.Write(buffer, 0, bytesRead);
					}
				} while (bytesRead > 0);
				
				fileStream.Flush();
			}
		}
	}
}
