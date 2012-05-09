using System;
using System.Collections.Generic;
using System.IO;

using Shouldly;
using TechTalk.SpecFlow;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class PathAlreadyExistsScenario
	{
		string pathThatAlreadyExists;
		StringWriter writer;
		EntryPoint entryPoint;
		ExitCode exitCode;
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			writer = new StringWriter();
			
			var log = new Log(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			var createCommands = new Dictionary<string, ICommand>();
			var fileSystem = new FileSystem();
			createCommands.Add("site", new CreateSiteCommand(log, fileSystem, new FileSystemUnified(log, fileSystem)));
			
			var commands = new Dictionary<string, ICommand>();
			commands.Add("create", new CreateCommand(log, createCommands));
			
			entryPoint = new EntryPoint(log, commands);
		}
		
		[Given(@"I have a path that already exists")]
		public void Given_I_have_a_path_that_already_exists()
		{
			pathThatAlreadyExists = Path.Combine(Path.GetTempPath());
		}
		
		[When(@"I run the create site command with a path that already exists")]
		public void When_I_run_the_create_site_command_with_path_that_already_exists()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site", pathThatAlreadyExists });
		}
		
		[Then(@"I should see ""A site at '\[path]' already exists\."" message")]
		public void Then_I_should_see_a_site_at_path_already_exists_message()
		{
			writer.ToString().ShouldContain(string.Format("A site at '{0}' already exists.", pathThatAlreadyExists));
		}
		
		[Then(@"I should see mulder terminate with an error exit code due to path existing")]
		public void Then_I_should_see_mulder_terminate_with_an_error_exit_code_due_to_path_existing()
		{
			exitCode.ShouldBe(ExitCode.Error);
		}
	}
}
