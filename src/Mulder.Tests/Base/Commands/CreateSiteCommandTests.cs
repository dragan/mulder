using System;
using System.IO;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Domain;
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
			ISite site;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				site = Substitute.For<ISite>();
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem, site);
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
			ISite site;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				site = Substitute.For<ISite>();
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem, site);
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
			ISite site;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				pathThatAlreadyExists = string.Format("path{0}that{0}already{0}exists", Path.DirectorySeparatorChar);
				
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				site = Substitute.For<ISite>();
				
				fileSystem.DirectoryExists(pathThatAlreadyExists).Returns(true);
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem, site);
			}
			
			[Test]
			public void should_call_directory_exists_on_given_path()
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
		
		[TestFixture]
		public class when_creating_a_site_with_a_valid_path
		{
			string validPath;
			StringWriter writer;
			IFileSystem fileSystem;
			ISite site;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				validPath = string.Format("some{0}valid{0}path", Path.DirectorySeparatorChar);
				
				writer = new StringWriter();
				fileSystem = Substitute.For<IFileSystem>();
				site = Substitute.For<ISite>();
				
				fileSystem.DirectoryExists(validPath).Returns(false);
				fileSystem.ChangeDirectory(validPath, Arg.Invoke());
				
				createSiteCommand = new CreateSiteCommand(writer, fileSystem, site);
			}
			
			[Test]
			public void should_create_valid_path_directory()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().CreateDirectory(validPath);
			}
			
			[Test]
			public void should_change_directory_into_valid_path()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().ChangeDirectory(validPath, Arg.Any<Action>());
			}
			
			[Test]
			public void should_create_default_config_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().WriteStreamToFile("config.yaml", Arg.Any<Stream>());
			}
			
			[Test]
			public void should_write_config_file_created()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				writer.ToString().ShouldContain("create config.yaml");
			}
			
			[Test]
			public void should_create_default_rules_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().WriteStreamToFile("Rules", Arg.Any<Stream>());
			}
			
			[Test]
			public void should_write_rules_file_created()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				writer.ToString().ShouldContain("create Rules");
			}
			
			[Test]
			public void should_create_default_layout_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				site.Received().CreateLayout("/default/", Arg.Any<Stream>());
			}
			
			[Test]
			public void should_create_default_home_page_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				site.Received().CreateItem("/", Arg.Any<Stream>(), Arg.Any<object>());
			}
			
			[Test]
			public void should_create_default_stylesheet_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				site.Received().CreateItem("/stylesheet/", Arg.Any<Stream>(), ".css");
			}
			
			[Test]
			public void should_create_lib_directory()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().CreateDirectory("lib");
			}
			
			[Test]
			public void should_write_site_created_message()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				writer.ToString().ShouldContain(string.Format("Created a blank mulder site at '{0}'. Enjoy!", validPath));
			}
		}
	}
}
