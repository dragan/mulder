using System;
using System.IO;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Commands
{
	public class CreateSiteCommandTests
	{
		[TestFixture]
		public class when_executing_with_wrong_number_of_arguments
		{
			ILog log;
			IFileSystem fileSystem;
			IDataSource dataSource;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				dataSource = Substitute.For<IDataSource>();
				
				createSiteCommand = new CreateSiteCommand(log, fileSystem, dataSource);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				createSiteCommand.Execute(new string[] {});
				
				log.Received().ErrorMessage("usage: {0}", createSiteCommand.Usage);
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
			ILog log;
			IFileSystem fileSystem;
			IDataSource dataSource;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				dataSource = Substitute.For<IDataSource>();
				
				createSiteCommand = new CreateSiteCommand(log, fileSystem, dataSource);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				createSiteCommand.Execute(new string[] { "" });
				
				log.Received().ErrorMessage("usage: {0}", createSiteCommand.Usage);
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
			ILog log;
			IFileSystem fileSystem;
			IDataSource dataSource;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				pathThatAlreadyExists = Path.Combine("path", "that", "already", "exists");
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				dataSource = Substitute.For<IDataSource>();
				
				fileSystem.DirectoryExists(pathThatAlreadyExists).Returns(true);
				
				createSiteCommand = new CreateSiteCommand(log, fileSystem, dataSource);
			}
			
			[Test]
			public void should_call_directory_exists_on_given_path()
			{
				createSiteCommand.Execute(new string[] { pathThatAlreadyExists });
				
				fileSystem.Received().DirectoryExists(pathThatAlreadyExists);
			}
			
			[Test]
			public void should_log_that_path_already_exists()
			{
				createSiteCommand.Execute(new string[] { pathThatAlreadyExists });
				
				log.Received().ErrorMessage("A site at '{0}' already exists.", pathThatAlreadyExists);
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
			ILog log;
			IFileSystem fileSystem;
			IDataSource dataSource;
			CreateSiteCommand createSiteCommand;
			
			[SetUp]
			public void SetUp()
			{
				validPath = Path.Combine("some", "valid", "path");
				
				log = Substitute.For<ILog>();
				fileSystem = Substitute.For<IFileSystem>();
				dataSource = Substitute.For<IDataSource>();
				
				fileSystem.DirectoryExists(validPath).Returns(false);
				fileSystem.ChangeDirectory(validPath, Arg.Invoke());
				
				createSiteCommand = new CreateSiteCommand(log, fileSystem, dataSource);
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
			public void should_log_config_file_created()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				log.Received().InfoMessage("\tcreate config.yaml");
			}
			
			[Test]
			public void should_create_default_rules_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().WriteStreamToFile("Rules", Arg.Any<Stream>());
			}
			
			[Test]
			public void should_log_rules_file_created()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				log.Received().InfoMessage("\tcreate Rules");
			}
			
			[Test]
			public void should_create_default_layout_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				dataSource.Received().CreateLayout("/default/", Arg.Any<Stream>());
			}
			
			[Test]
			public void should_create_default_home_page_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				dataSource.Received().CreateItem("/", Arg.Any<Stream>(), Arg.Any<object>());
			}
			
			[Test]
			public void should_create_default_stylesheet_file()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				dataSource.Received().CreateItem("/stylesheet/", Arg.Any<Stream>(), ".css");
			}
			
			[Test]
			public void should_create_lib_directory()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				fileSystem.Received().CreateDirectory("lib");
			}
			
			[Test]
			public void should_log_site_created_message()
			{
				createSiteCommand.Execute(new string[] { validPath });
				
				log.Received().InfoMessage("Created a blank mulder site at '{0}'. Enjoy!", validPath);
			}
		}
	}
}
