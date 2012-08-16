using System;
using System.IO;
using System.Reflection;

using Mulder.Base.IO;

namespace Mulder.Acceptance.Tests.Helpers
{
	public class TestFileCreation
	{
		public static void CreateFileFromResourceName(string resourceName, string path)
		{
			var fileSystem = new FileSystem();
			using (var resourceStream = GetResourceStreamFromResourceName(resourceName)) {
				fileSystem.WriteStreamToFile(path, resourceStream);
			}
		}
		
		static Stream GetResourceStreamFromResourceName(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}
	}
}
