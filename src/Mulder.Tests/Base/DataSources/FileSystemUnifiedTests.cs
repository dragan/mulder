using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.DataSources;
using Mulder.Base.Domain;
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
			IDictionary<string, object> configuration;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				configuration = Substitute.For<IDictionary<string, object>>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
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
			IDictionary<string, object> configuration;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				configuration = Substitute.For<IDictionary<string, object>>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
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
			IDictionary<string, object> configuration;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				content = new MemoryStream();
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				configuration = Substitute.For<IDictionary<string, object>>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
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
			IDictionary<string, object> configuration;
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
				configuration = Substitute.For<IDictionary<string, object>>();
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
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
		
		[TestFixture]
		public class when_getting_items
		{
			DateTime expectedModificationTime;
			ILog log;
			IFileSystem fileSystem;
			IDictionary<string, object> configuration;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				var fakeFiles = CreateFakeFileList();
				expectedModificationTime = DateTime.UtcNow;
				
				log = Substitute.For<ILog>();
				
				fileSystem = Substitute.For<IFileSystem>();
				fileSystem.GetAllFiles("content").Returns(fakeFiles);
				fileSystem.ReadStringFromFile("content/index.html").Returns("---\nnum: 1\n---\ntest 1");
				fileSystem.ReadStringFromFile("content/a.html").Returns("---\nnum: 2\n---\ntest 2");
				fileSystem.ReadStringFromFile("content/a/b.html").Returns("---\nnum: 3\n---\ntest 3");
				fileSystem.ReadStringFromFile("content/a/b/c.html").Returns("test 4");
				fileSystem.ReadStringFromFile("content/a/b/c.yaml").Returns("num: 4");
				fileSystem.GetLastWriteTimeUtc(Arg.Any<string>()).Returns(expectedModificationTime);
				
				configuration = Substitute.For<IDictionary<string, object>>();
				configuration["TextExtensions"].Returns(new string[] { "html", "yaml" });
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
			}
			
			[Test]
			public void should_not_retrieve_certain_files()
			{
				IEnumerable<Item> items = fileSystemUnified.GetItems();
				
				items.Count().ShouldBe(5);
			}
			
			[Test]
			public void should_return_expected_items_based_on_fake_files()
			{
				IEnumerable<Item> expectedItems = CreateExpectedItems();
				
				IEnumerable<Item> items = fileSystemUnified.GetItems();
				
				items.ShouldBe(expectedItems);
			}
			
			IEnumerable<string> CreateFakeFileList()
			{
				return new List<string> {
					Path.Combine("content", "~bad1.txt"),
					Path.Combine("content", "bad2.orig"),
					Path.Combine("content", "bad3.rej"),
					Path.Combine("content", "bad4.bak"),
					Path.Combine("content", "index.html"),
					Path.Combine("content", "a.html"),
					Path.Combine("content", "a", "b.html"),
					Path.Combine("content", "a", "b", "c.html"),
					Path.Combine("content", "a", "b", "c.yaml"),
					Path.Combine("content", "binary.dat")
				};
			}
			
			IEnumerable<Item> CreateExpectedItems()
			{
				return new List<Item> {
					new Item("/",
						false,
						"test 1",
						new Dictionary<string, object> {
							{ "filename", "content/index.html" },
							{ "meta_filename", "" },
							{ "extension", ".html" },
							{ "num", "1" }
						},
						expectedModificationTime),
					new Item("/a/",
						false,
						"test 2",
						new Dictionary<string, object> {
							{ "filename", "content/a.html" },
							{ "meta_filename", "" },
							{ "extension", ".html" },
							{ "num", "2" }
						},
						expectedModificationTime),
					new Item("/a/b/",
						false,
						"test 3",
						new Dictionary<string, object> {
							{ "filename", "content/a/b.html" },
							{ "meta_filename", "" },
							{ "extension", ".html" },
							{ "num", "3" }
						},
						expectedModificationTime),
					new Item("/a/b/c/",
						false,
						"test 4",
						new Dictionary<string, object> {
							{ "filename", "content/a/b/c.html" },
							{ "meta_filename", "content/a/b/c.yaml" },
							{ "extension", ".html" },
							{ "num", "4" }
						},
						expectedModificationTime),
					new Item("/binary/",
						true,
						"",
						new Dictionary<string, object> {
							{ "filename", "content/binary.dat" },
							{ "meta_filename", "" },
							{ "extension", ".dat" }
						},
						expectedModificationTime)
				};
			}
		}
		
		[TestFixture]
		public class when_getting_layouts
		{
			DateTime expectedModificationTime;
			ILog log;
			IFileSystem fileSystem;
			IDictionary<string, object> configuration;
			FileSystemUnified fileSystemUnified;
			
			[SetUp]
			public void SetUp()
			{
				var fakeFiles = CreateFakeFileList();
				expectedModificationTime = DateTime.UtcNow;
				
				log = Substitute.For<ILog>();
				
				fileSystem = Substitute.For<IFileSystem>();
				fileSystem.GetAllFiles("layouts").Returns(fakeFiles);
				fileSystem.ReadStringFromFile("layouts/a.html").Returns("---\nnum: 1\n---\ntest 1");
				fileSystem.ReadStringFromFile("layouts/b.html").Returns("test 2");
				fileSystem.ReadStringFromFile("layouts/b.yaml").Returns("num: 2");
				fileSystem.GetLastWriteTimeUtc(Arg.Any<string>()).Returns(expectedModificationTime);
				
				configuration = Substitute.For<IDictionary<string, object>>();
				configuration["TextExtensions"].Returns(new string[] { "html", "yaml" });
				
				fileSystemUnified = new FileSystemUnified(log, fileSystem, configuration);
			}
			
			[Test]
			public void should_return_expected_layouts_based_on_fake_files()
			{
				IEnumerable<Layout> expectedLayouts = CreateExpectedLayouts();
				
				IEnumerable<Layout> layouts = fileSystemUnified.GetLayouts();
				
				layouts.ShouldBe(expectedLayouts);
			}
			
			IEnumerable<string> CreateFakeFileList()
			{
				return new List<string> {
					Path.Combine("layouts", "a.html"),
					Path.Combine("layouts", "b.html"),
					Path.Combine("layouts", "b.yaml")
				};
			}
			
			IEnumerable<Layout> CreateExpectedLayouts()
			{
				return new List<Layout> {
					new Layout("/a/",
						"test 1",
						new Dictionary<string, object> {
							{ "filename", "layouts/a.html" },
							{ "meta_filename", "" },
							{ "extension", ".html" },
							{ "num", "1" }
						},
						expectedModificationTime),
					new Layout("/b/",
						"test 2",
						new Dictionary<string, object> {
							{ "filename", "layouts/b.html" },
							{ "meta_filename", "layouts/b.yaml" },
							{ "extension", ".html" },
							{ "num", "2" }
						},
						expectedModificationTime)
				};
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
