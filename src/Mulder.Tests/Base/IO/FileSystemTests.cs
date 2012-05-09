using System;
using System.IO;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.IO;

namespace Mulder.Tests.Base.IO
{
	public class FileSystemTests
	{
		[TestFixture]
		public class when_wanting_to_execute_a_action_in_a_different_directory
		{
			string path;
			string originalDirectory;
			bool changedDirectories;
			bool actionExecuted;
			FileSystem fileSystem;
			
			[SetUp]
			public void Init()
			{
				path = "mulder-test-directory";
				originalDirectory = Directory.GetCurrentDirectory();
				changedDirectories = false;
				actionExecuted = false;
				Directory.CreateDirectory(path);
				
				fileSystem = new FileSystem();
			}
			
			[TearDown]
			public void CleanUp()
			{
				if (Directory.Exists(path))
					Directory.Delete(path, true);
			}
			
			[Test]
			public void should_change_current_directory_to_path()
			{
				fileSystem.ChangeDirectory(path, () => {
					string currentDirectory = Directory.GetCurrentDirectory();
					changedDirectories = currentDirectory == Path.Combine(originalDirectory, path);
				});
				
				changedDirectories.ShouldBe(true);
			}
			
			[Test]
			public void should_execute_action()
			{
				fileSystem.ChangeDirectory(path, () => {
					actionExecuted = true;
				});
				
				actionExecuted.ShouldBe(true);
			}
			
			[Test]
			public void should_return_to_original_directory_after_action_is_executed()
			{
				fileSystem.ChangeDirectory(path, () => {
					// Do Nothing
				});
				
				Directory.GetCurrentDirectory().ShouldBe(originalDirectory);
			}
		}
		
		[TestFixture]
		public class when_writing_a_stream_to_a_file
		{
			string basePath;
			string randomName;
			FileSystem fileSystem;
			
			[SetUp]
			public void SetUp()
			{
				basePath = Path.GetTempPath();
				randomName = Path.GetRandomFileName();
				fileSystem = new FileSystem();
			}
			
			[TearDown]
			public void CleanUp()
			{
				string testPath = Path.Combine(basePath, randomName);
				if (Directory.Exists(testPath))
					Directory.Delete(testPath, true);
			}
			
			[Test]
			public void should_create_folders_that_do_not_exist()
			{
				var pathDoesNotExist = Path.Combine(basePath, randomName);
				var stream = new MemoryStream(new byte[] { 10, 20, 30, 40 });

				fileSystem.WriteStreamToFile(Path.Combine(pathDoesNotExist, "file.txt"), stream);

				Directory.Exists(pathDoesNotExist).ShouldBe(true);
			}

			[Test]
			public void should_be_able_to_write_a_large_file()
			{
				const string text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur varius arcu eget nisi porta sit amet aliquet enim laoreet. Mauris at lorem velit, in venenatis augue. Pellentesque dapibus eros ac ipsum rutrum varius. Mauris non velit euismod odio tincidunt fermentum eget a enim. Pellentesque in erat nisl, consectetur lacinia leo. Suspendisse hendrerit blandit justo, sed aliquet libero eleifend sed. Fusce nisi tortor, ultricies sed tempor sit amet, viverra at quam. Vivamus sem mi, semper nec cursus vel, vehicula sit amet nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Cras commodo commodo tortor congue bibendum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Pellentesque vel magna vitae dui accumsan venenatis. Nullam sed ante mauris, nec iaculis erat. Cras eu nibh vel ante adipiscing volutpat. Integer ullamcorper tempus facilisis. Vestibulum eu magna sit amet dolor condimentum vestibulum non a ligula. Nunc purus nibh amet.";
				var path = Path.Combine(basePath, randomName);

				var memoryStream = new MemoryStream();
				var streamWriter = new StreamWriter(memoryStream);
				for (var i = 0; i < FileSystem.BufferSize / 512; i++) {
					streamWriter.Write(text);
				}
				memoryStream.Position = 0;

				fileSystem.WriteStreamToFile(path, memoryStream);

				var fileInfo = new FileInfo(path);
				fileInfo.Exists.ShouldBe(true);
				fileInfo.Length.ShouldBe(memoryStream.Length);
			}
		}
	}
}
