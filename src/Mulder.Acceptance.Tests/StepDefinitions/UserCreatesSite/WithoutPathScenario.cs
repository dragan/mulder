using System;
using System.Collections.Generic;
using System.IO;

using Autofac;
using TechTalk.SpecFlow;
using Shouldly;

using Mulder.Acceptance.Tests.Helpers;
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
		
		[BeforeScenario]
		public void BeforeScenario()
		{
			writer = new StringWriter();
			
			IContainer container = Ioc.CreateAcceptanceTestsContainer(writer, new LogLevel[] { LogLevel.Info, LogLevel.Error });
			
			entryPoint = container.Resolve<EntryPoint>();
		}
		
		[When(@"I run the create site command without a path")]
		public void When_I_run_the_create_site_command_without_a_path()
		{
			exitCode = entryPoint.Run(new string[] { "create", "site" });
		}
		
		[Then(@"I should see usage message")]
		public void Then_I_should_see_usage_message()
		{
			writer.ToString().ShouldContain("usage: mulder create site <path>");
		}
		
		[Then(@"I should see mulder terminate with an error exit code")]
		public void Then_I_should_see_mulder_terminate_with_an_error_exit_code()
		{
			exitCode.ShouldBe(ExitCode.Error);
		}
	}
}
