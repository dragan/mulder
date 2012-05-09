using System;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base
{
	public class EntryPointTests
	{
		[TestFixture]
		public class when_running_with_no_arguments
		{
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				commands = Substitute.For<IDictionary<string, ICommand>>();
				
				entryPoint = new EntryPoint(log, commands);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				entryPoint.Run(new string[] {});
				
				log.Received().ErrorMessage("usage: [command]");
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_executing_with_invalid_command_argument
		{
			string invalidArgument;
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				invalidArgument = "blah";
				log = Substitute.For<ILog>();
				commands = Substitute.For<IDictionary<string, ICommand>>();
				
				commands.ContainsKey(invalidArgument).Returns(false);
				
				entryPoint = new EntryPoint(log, commands);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				entryPoint.Run(new string[] { invalidArgument });
				
				log.Received().ErrorMessage("usage: [command]");
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { invalidArgument });
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_running_with_valid_command_argument
		{
			string commandArgument;
			string additionalArgument;
			ICommand command;
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				commandArgument = "command";
				additionalArgument = "additionalArgument";
				
				command = Substitute.For<ICommand>();
				command.Execute(Arg.Any<string[]>()).Returns(ExitCode.Success);
				
				log = Substitute.For<ILog>();
				
				commands = Substitute.For<IDictionary<string, ICommand>>();
				commands.ContainsKey(commandArgument).Returns(true);
				commands[commandArgument] = command;
				
				entryPoint = new EntryPoint(log, commands);
			}
			
			[Test]
			public void should_call_execute_on_command_with_no_arguments()
			{
				entryPoint.Run(new string[] { commandArgument });
				
				command.Received().Execute(Arg.Is<string[]>(args => args.Length == 0));
			}
			
			[Test]
			public void should_call_execute_on_command_with_additional_arguments()
			{
				entryPoint.Run(new string[] { commandArgument, additionalArgument });
				
				command.Received().Execute(Arg.Is<string[]>(args => args[0] == additionalArgument));
			}
			
			[Test]
			public void should_return_command_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { commandArgument });
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}
	}
}
