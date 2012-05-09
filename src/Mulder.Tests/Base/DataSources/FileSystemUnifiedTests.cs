using System;
using System.IO;
using System.Linq;
using System.Text;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.DataSources
{
	public class FileSystemUnifiedTests
	{
		[TestFixture]
		public class when_creating_a_layout_with_default_extension
		{
			MemoryStream content;
			ILog log;
			IFileSystem fileSystem;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem);
			}
			
			[Test]
			public void should_create_layouts_directory()
			{
				fileSystemUnified.CreateLayout("/file/", content);
				
				fileSystem.Received().CreateDirectory("layouts");
			}
			
			[Test]
			public void should_create_file_based_on_identifier()
			{
				fileSystemUnified.CreateLayout("/file/", content);
				
				fileSystem.Received().WriteStreamToFile(Path.Combine("layouts", "file.html"), Arg.Any<MemoryStream>());
			}
			
			[Test]
			public void should_log_file_created()
			{
				fileSystemUnified.CreateLayout("/file/", content);
				
				log.Received().InfoMessage("\tcreate {0}", Path.Combine("layouts", "file.html"));
			}
		}
		
		[TestFixture]
		public class when_creating_an_item_with_extension
		{
			MemoryStream content;
			ILog log;
			IFileSystem fileSystem;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem);
			}
			
			[Test]
			public void should_create_content_directory()
			{
				fileSystemUnified.CreateItem("/file/", content, ".txt");
				
				fileSystem.Received().CreateDirectory("content");
			}
			
			[Test]
			public void should_create_file_based_on_identifier()
			{
				fileSystemUnified.CreateItem("/file/", content, ".txt");
				
				fileSystem.Received().WriteStreamToFile(Path.Combine("content", "file.txt"), Arg.Any<MemoryStream>());
			}
			
			[Test]
			public void should_log_file_created()
			{
				fileSystemUnified.CreateItem("/file/", content, ".txt");
				
				log.Received().InfoMessage("\tcreate {0}", Path.Combine("content", "file.txt"));
			}
		}
		
		[TestFixture]
		public class when_creating_an_item_with_default_extension
		{
			MemoryStream content;
			ILog log;
			IFileSystem fileSystem;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem);
			}
			
			[Test]
			public void should_create_content_directory()
			{
				fileSystemUnified.CreateItem("/", content);
				
				fileSystem.Received().CreateDirectory("content");
			}
			
			[Test]
			public void should_create_default_file_based_on_identifier()
			{
				fileSystemUnified.CreateItem("/", content);
				
				fileSystem.Received().WriteStreamToFile(Path.Combine("content", "index.html"), Arg.Any<MemoryStream>());
			}
			
			[Test]
			public void should_log_file_created()
			{
				fileSystemUnified.CreateItem("/", content);
				
				log.Received().InfoMessage("\tcreate {0}", Path.Combine("content", "index.html"));
			}
		}
		
		[TestFixture]
		public class when_creating_an_item_with_a_meta_object
		{
			MemoryStream itemContent;
			MemoryStream expected;
			ILog log;
			IFileSystem fileSystem;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				var itemContentString = Encoding.UTF8.GetBytes("Item Content");
				itemContent = new MemoryStream(itemContentString);
				
				var expectedString = Encoding.UTF8.GetBytes(@"---
property1: Property1
property2: Property2
---

Item Content");
				
				expected = new MemoryStream();
				expected.Write(expectedString, 0, expectedString.Length);
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem);
			}
			
			[Test]
			public void should_create_content_directory()
			{
				fileSystemUnified.CreateItem("/file/", itemContent, ".txt", new { Property1 = "Property1", Property2 = "Property2" });
				
				fileSystem.Received().CreateDirectory("content");
			}
			
			[Test]
			public void should_create_file_based_on_identifier()
			{
				fileSystemUnified.CreateItem("/file/", itemContent, ".txt");
				
				fileSystem.Received().WriteStreamToFile(Path.Combine("content", "file.txt"), Arg.Any<MemoryStream>());
			}
			
			[Test]
			public void should_log_file_created()
			{
				fileSystemUnified.CreateItem("/file/", itemContent, ".txt");
				
				log.Received().InfoMessage("\tcreate {0}", Path.Combine("content", "file.txt"));
			}
		}
	}
	
	public static class MemoryStreamExtensions
	{
		public static bool IsEqualTo(this MemoryStream ms1, MemoryStream ms2)
		{
			if (ms1.Length != ms2.Length)
				return false;
			
			ms1.Position = 0;
			ms2.Position = 0;

			var msArray1 = ms1.ToArray();
			var msArray2 = ms2.ToArray();

			return msArray1.SequenceEqual(msArray2);
		}
	}
}

