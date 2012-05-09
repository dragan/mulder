using System;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Commands;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Commands
{
	public class CreateCommandTests
	{
		[TestFixture]
		public class when_executing_with_wrong_number_of_arguments
		{
			ILog log;
			IDictionary<string, ICommand> subCommands;
			CreateCommand createCommand;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				subCommands = Substitute.For<IDictionary<string, ICommand>>();
				
				createCommand = new CreateCommand(log, subCommands);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				createCommand.Execute(new string[] {});
				
				log.Received().ErrorMessage("usage: {0}", createCommand.Usage);
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = createCommand.Execute(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_executing_with_invalid_sub_command_argument
		{
			string invalidArgument;
			ILog log;
			IDictionary<string, ICommand> subCommands;
			CreateCommand createCommand;
			
			[SetUp]
			public void SetUp()
			{
				invalidArgument = "blah";
				log = Substitute.For<ILog>();
				subCommands = Substitute.For<IDictionary<string, ICommand>>();
				
				subCommands.ContainsKey(invalidArgument).Returns(false);
				
				createCommand = new CreateCommand(log, subCommands);
			}
			
			[Test]
			public void should_log_usage_message()
			{
				createCommand.Execute(new string[] { invalidArgument });
				
				log.Received().ErrorMessage("usage: {0}", createCommand.Usage);
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = createCommand.Execute(new string[] { invalidArgument });
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}
		
		[TestFixture]
		public class when_executing_with_valid_sub_command_argument
		{
			string subCommandArgument;
			string additionalArgument;
			ICommand subCommand;
			ILog log;
			IDictionary<string, ICommand> subCommands;
			CreateCommand createCommand;
			
			[SetUp]
			public void SetUp()
			{
				subCommandArgument = "subCommand";
				additionalArgument = "additionalArgument";
				
				subCommand = Substitute.For<ICommand>();
				subCommand.Execute(Arg.Any<string[]>()).Returns(ExitCode.Success);
				
				log = Substitute.For<ILog>();
				
				subCommands = Substitute.For<IDictionary<string, ICommand>>();
				subCommands.ContainsKey(subCommandArgument).Returns(true);
				subCommands[subCommandArgument] = subCommand;
				
				createCommand = new CreateCommand(log, subCommands);
			}
			
			[Test]
			public void should_call_execute_on_sub_command_with_no_arguments()
			{
				createCommand.Execute(new string[] { subCommandArgument });
				
				subCommand.Received().Execute(Arg.Is<string[]>(args => args.Length == 0));
			}
			
			[Test]
			public void should_call_execute_on_sub_command_with_additional_arguments()
			{
				createCommand.Execute(new string[] { subCommandArgument, additionalArgument });
				
				subCommand.Received().Execute(Arg.Is<string[]>(args => args[0] == additionalArgument));
			}
			
			[Test]
			public void should_return_sub_command_exit_code()
			{
				ExitCode exitCode = createCommand.Execute(new string[] { subCommandArgument });
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}
	}
}
