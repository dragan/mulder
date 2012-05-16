using System;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.Loading;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Commands
{
	public class CompileCommandTests
	{
		[TestFixture]
		public class when_compiling_a_valid_mulder_site
		{
			ILog log;
			ILoader loader;
			ICompiler compiler;
			CompileCommand compileCommand;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				loader = Substitute.For<ILoader>();
				compiler = Substitute.For<ICompiler>();
				
				compileCommand = new CompileCommand(log, loader, compiler);
			}
			
			[Test]
			public void should_log_loading_site_data()
			{
				compileCommand.Execute(new string[] {});
				
				log.Received().InfoMessage("Loading site data...");
			}
			
			[Test]
			public void should_call_load_site_data()
			{
				compileCommand.Execute(new string[] {});
				
				loader.Received().LoadSiteData();
			}
			
			[Test]
			public void should_log_compiling_site()
			{
				compileCommand.Execute(new string[] {});
				
				log.Received().InfoMessage("Compiling site...");
			}
			
			[Test]
			public void should_call_compile_with_a_site()
			{
				compileCommand.Execute(new string[] {});
				
				compiler.Received().Compile(Arg.Any<Site>());
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
