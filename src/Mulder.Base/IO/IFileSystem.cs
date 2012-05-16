using System;
using System.Collections.Generic;
using System.IO;

namespace Mulder.Base.IO
{
	public interface IFileSystem
	{
		bool DirectoryExists(string path);
		void CreateDirectory(string path);
		void ChangeDirectory(string path, Action action);
		void WriteStreamToFile(string filename, Stream stream);
		void WriteStringToFile(string filename, string text);
		IEnumerable<string> GetAllFiles(string path);
		string ReadStringFromFile(string filename);
		DateTime GetLastWriteTimeUtc(string filename);
		bool FileExists(string filename);
	}
}
