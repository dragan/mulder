using System;
using System.IO;

namespace Mulder.Base.IO
{
	public interface IFileSystem
	{
		bool DirectoryExists(string directory);
		void CreateDirectory(string directory);
		void ChangeDirectory(string directory, Action action);
		void WriteStreamToFile(string filename, Stream stream);
	}
}
