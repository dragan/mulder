using System;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Extensions;

namespace Mulder.Tests.Base.Extensions
{
	public class FileExtensionsTests
	{
		[TestFixture]
		public class when_removing_path_from_filename
		{
			[Test]
			public void should_return_filename_without_path()
			{
				string path = "foo";
				string fileName = "foo/bar/foobar.html";
				
				fileName.RemovePathFromFileName(path).ShouldBe("bar/foobar.html");
			}
		}
	}
}
