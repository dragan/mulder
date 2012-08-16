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
			public void should_log_error_message()
			{
				entryPoint.Run(new string[] {});
				
				log.Received().ErrorMessage(Messages.NoArgumentsMessage);
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
			public void should_log_error_message()
			{
				entryPoint.Run(new string[] { invalidArgument });

				log.Received().ErrorMessage(Messages.InvalidCommandMessage, invalidArgument);
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { invalidArgument });
				
				exitCode.ShouldBe(ExitCode.Error);
			}
		}

		[TestFixture]
		public class when_running_with_help_option_argument
		{
			string helpOptionArgumentShort;
			string helpOptionArgumentQuestionMark;
			string helpOptionArgumentLong;
			string helpCommand;
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				helpOptionArgumentShort = "-h";
				helpOptionArgumentQuestionMark = "-?";
				helpOptionArgumentLong = "--help";
				helpCommand = "help";

				log = Substitute.For<ILog>();
				commands = Substitute.For<IDictionary<string, ICommand>>();

				entryPoint = new EntryPoint(log, commands);
			}

			[Test]
			public void should_log_help_message_for_short_argument()
			{
				entryPoint.Run(new string[] { helpOptionArgumentShort });

				log.Received().InfoMessage(Messages.Help);
			}
			
			[Test]
			public void should_return_success_exit_code_for_short_argument()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { helpOptionArgumentShort });
				
				exitCode.ShouldBe(ExitCode.Success);
			}

			[Test]
			public void should_log_help_message_for_question_mark_argument()
			{
				entryPoint.Run(new string[] { helpOptionArgumentQuestionMark });

				log.Received().InfoMessage(Messages.Help);
			}

			[Test]
			public void should_return_success_exit_code_for_question_mark_argument()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { helpOptionArgumentQuestionMark });
				
				exitCode.ShouldBe(ExitCode.Success);
			}

			[Test]
			public void should_log_help_message_for_long_argument()
			{
				entryPoint.Run(new string[] { helpOptionArgumentLong });

				log.Received().InfoMessage(Messages.Help);
			}
			
			[Test]
			public void should_return_success_exit_code_for_long_argument()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { helpOptionArgumentLong });
				
				exitCode.ShouldBe(ExitCode.Success);
			}

			[Test]
			public void should_log_help_message_for_help_command()
			{
				entryPoint.Run(new string[] { helpCommand });

				log.Received().InfoMessage(Messages.Help);
			}
			
			[Test]
			public void should_return_success_exit_code_for_help_command()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { helpCommand });
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}

		[TestFixture]
		public class when_requesting_help_on_a_specific_command
		{
			string helpCommand;
			string commandArgument;
			ICommand command;
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;

			[SetUp]
			public void SetUp()
			{
				helpCommand = "help";
				commandArgument = "command";

				command = Substitute.For<ICommand>();

				log = Substitute.For<ILog>();

				commands = Substitute.For<IDictionary<string, ICommand>>();
				commands.ContainsKey(commandArgument).Returns(true);
				commands[commandArgument] = command;

				entryPoint = new EntryPoint(log, commands);
			}

			[Test]
			public void should_call_usage_on_command()
			{
				entryPoint.Run(new string[] { helpCommand, commandArgument });

				command.Received().ShowHelp(Arg.Any<string[]>());
			}
		}

		[TestFixture]
		public class when_requesting_help_with_invalid_command_argument
		{
			string helpCommand;
			string invalidArgument;
			ILog log;
			IDictionary<string, ICommand> commands;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				helpCommand = "help";
				invalidArgument = "blah";

				log = Substitute.For<ILog>();

				commands = Substitute.For<IDictionary<string, ICommand>>();
				commands.ContainsKey(invalidArgument).Returns(false);

				entryPoint = new EntryPoint(log, commands);
			}
			
			[Test]
			public void should_log_error_message()
			{
				entryPoint.Run(new string[] { helpCommand, invalidArgument });

				log.Received().ErrorMessage(Messages.InvalidCommandMessage, invalidArgument);
			}
			
			[Test]
			public void should_return_error_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] { helpCommand, invalidArgument });
				
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

		public class Messages
		{
			public const string NoArgumentsMessage = "mulder: You must provide a command. Run 'mulder help' for more info.";
			public const string InvalidCommandMessage = "mulder: unknown command '{0}'. Run 'mulder help' for more info.";
			public const string Help = @"
mulder, a simple static site generator written in C#

usage: mulder [options] <command> [<args>]

commands:


    See 'mulder help <command>' for more information on a specific command.

options:

    -h -? --help    show the help message and quit
";
		}
	}
}
