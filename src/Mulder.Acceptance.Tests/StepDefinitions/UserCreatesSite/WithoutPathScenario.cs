using System;
using System.IO;

using TechTalk.SpecFlow;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Acceptance.Tests.StepDefinitions.UserCreatesSite
{
	[Binding]
    public class WithoutPathScenario
	{
		StringWriter writer;
		EntryPoint entryPoint;
		ExitCode exitCode;
		
		[BeforeStep]
		public void BeforeStep()
		{
			writer = new StringWriter();
			entryPoint = new EntryPoint(new Log(writer, new LogLevel[] { LogLevel.Error }));
		}
		
		[When(@"I run the create site command without a path")]
		public void When_I_run_the_create_site_command_without_a_path()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site" });
		}
		
		[Then(@"I should see usage message")]
		public void Then_I_should_see_usage_message()
		{
			ScenarioContext.Current.Pending();
		}
		
		[Then(@"I should see mulder terminate with an error exit code")]
		public void Then_I_should_see_mulder_terminate_with_an_error_exit_code()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
