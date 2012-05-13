using System;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Commands
{
	public class CompileCommandTests
	{
		[TestFixture]
		public class when_compiling_a_valid_mulder_site
		{
			ILog log;
			CompileCommand compileCommand;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				
				compileCommand = new CompileCommand(log);
			}
			
			[Test]
			public void should_log_loading_site_data()
			{
				compileCommand.Execute(new string[] {});
				
				log.Received().InfoMessage("Loading site data...");
			}
			
			[Test]
			public void should_log_compiling_site()
			{
				compileCommand.Execute(new string[] {});
				
				log.Received().InfoMessage("Compiling site...");
			}
			
			[Test]
			public void should_log_compiled_message()
			{
				compileCommand.Execute(new string[] {});
				
				log.Received().InfoMessage("Site compiled in {0}s.", Arg.Any<int>());
			}
			
			[Test]
			public void should_return_success_exit_code()
			{
				ExitCode exitCode = compileCommand.Execute(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}
	}
}
