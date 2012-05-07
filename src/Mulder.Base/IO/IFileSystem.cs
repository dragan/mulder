using System;

namespace Mulder.Base.IO
{
	public interface IFileSystem
	{
		bool DirectoryExists(string directory);
	}
}
