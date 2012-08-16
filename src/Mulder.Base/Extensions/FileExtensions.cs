using System;

namespace Mulder.Base.Extensions
{
	public static class FileExtensions
	{
		public static string RemovePathFromFileName(this string fileName, string path)
		{
			return fileName.Substring(path.Length + 1);
		}
	}
}

