using System;
using System.Collections.Generic;
using System.IO;

using TechTalk.SpecFlow;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.DataSources;
using Mulder.Base.IO;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class WithoutPathScenario
	{
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
		
		[When(@"I run the create site command without a path")]
		public void When_I_run_the_create_site_command_without_a_path()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site" });
		}
		
		[Then(@"I should see usage message")]
		public void Then_I_should_see_usage_message()
		{
			writer.ToString().ShouldContain("usage: create site [path]");
		}
		
		[Then(@"I should see mulder terminate with an error exit code")]
		public void Then_I_should_see_mulder_terminate_with_an_error_exit_code()
		{
			exitCode.ShouldBe(ExitCode.Error);
		}
	}
}
