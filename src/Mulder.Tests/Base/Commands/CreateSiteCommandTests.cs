using System;
using System.IO;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.IO;

namespace Mulder.Tests.Base.Commands
{
	public class CreateSiteCommandTests
	{
		[TestFixture]
		public class when_executing_with_wrong_number_of_arguments
		{
			StringWriter writer;
			IFileSystem fileSystem;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem);
			}
			
			[Test]
			public void should_write_usage()
			{
				createSiteCommand.Execute(new string[] {});
				
				writer.ToString().ShouldContain("usage: create site [path]");
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = createSiteCommand.Execute(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_executing_with_empty_string_for_path
		{
			StringWriter writer;
			IFileSystem fileSystem;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem);
			}
			
			[Test]
			public void should_write_usage()
			{
				createSiteCommand.Execute(new string[] { "" });
				
				writer.ToString().ShouldContain("usage: create site [path]");
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = createSiteCommand.Execute(new string[] { "" });
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_creating_a_site_with_a_path_that_already_exists
		{
			string pathThatAlreadyExists;
			StringWriter writer;
			IFileSystem fileSystem;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				pathThatAlreadyExists = string.Format("path{0}that{0}already{0}exists", Path.DirectorySeparatorChar);
				
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				
				fileSystem.DirectoryExists(pathThatAlreadyExists).Returns(true);
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem);
			}
			
			[Test]
			public void should_call_directory_exists_on_file_system()
			{
				createSiteCommand.Execute(new string[] { pathThatAlreadyExists });
				
				fileSystem.Received().DirectoryExists(pathThatAlreadyExists);
			}
			
			[Test]
			public void should_write_that_path_already_exists()
			{
				createSiteCommand.Execute(new string[] { pathThatAlreadyExists });
				
				writer.ToString().ShouldContain(string.Format("A site at '{0}' already exists.", pathThatAlreadyExists));
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = createSiteCommand.Execute(new string[] { pathThatAlreadyExists });
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
	}
}
