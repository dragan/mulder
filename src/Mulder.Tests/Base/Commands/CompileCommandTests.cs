using System;
using System.CodeDom.Compiler;
using System.Text;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Compilation;
using Mulder.Base.Domain;
using Mulder.Base.Exceptions;
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
				
				log.Received().InfoMessage("Site compiled in {0}s.", Arg.Any<string>());
			}
			
			[Test]
			public void should_return_success_exit_code()
			{
				ExitCode exitCode = compileCommand.Execute(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}

		[TestFixture]
		public class when_showing_help
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
			public void should_log_help()
			{
				compileCommand.ShowHelp(new string[] {});

				log.Received().InfoMessage(Messages.BuildHelpMessage());
			}

			[Test]
			public void should_return_success_exit_code()
			{
				ExitCode exitCode = compileCommand.ShowHelp(new string[] {});

				exitCode.ShouldBe(ExitCode.Success);
			}
		}

		[TestFixture]
		public class when_load_site_data_throws_compiling_rules_exception
		{
			ILog log;
			ILoader loader;
			ICompiler compiler;
			CompilerError compilerError;
			CompileCommand compileCommand;

			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				compiler = Substitute.For<ICompiler>();
				loader = Substitute.For<ILoader>();

				compilerError = new CompilerError("filename.cs", 50, 2, "error CS1525", "Unexpected symbol `}', expecting `;'");

				var errorCollection = new CompilerErrorCollection();
				errorCollection.Add(compilerError);

				loader.LoadSiteData().Returns(x => { throw new ErrorCompilingRulesException(errorCollection); });

				compileCommand = new CompileCommand(log, loader, compiler);
			}

			[Test]
			public void should_log_message()
			{
				compileCommand.Execute(new string[] {});

				log.Received().ErrorMessage("There was a problem compiling the Rules");
			}

			[Test]
			public void should_log_error()
			{
				compileCommand.Execute(new string[] {});

				int line = compilerError.Line - 28;
				log.Received().ErrorMessage("   Line {0}: {1} {2}", line, compilerError.ErrorNumber, compilerError.ErrorText);
			}

			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = compileCommand.Execute(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}

		public class Messages
		{
			public static string BuildHelpMessage()
			{
				var sb = new StringBuilder();

				sb.AppendLine();
				sb.AppendLine("usage: mulder compile");
				sb.AppendLine();
				sb.AppendLine("compile items of this site");
				sb.AppendLine();
				sb.AppendLine("    Compile all items of the current site.");

				return sb.ToString();
			}
		}
	}
}
