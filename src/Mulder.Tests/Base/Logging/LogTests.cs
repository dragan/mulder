using System;
using System.IO;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Logging;

namespace Mulder.Tests.Base.Logging
{
	public class LogTests
	{
		public class when_logging_debug_messages
		{
			StringWriter writer;
			Log log;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				log = new Log(writer, new [] { LogLevel.Debug });
			}
			
			[Test]
			public void should_write_message_to_console()
			{
				log.DebugMessage("Debug Message");
				
				writer.ToString().ShouldContain("Debug Message");
			}
			
			[Test]
			public void should_write_message_with_params_to_console()
			{
				log.DebugMessage("Debug Message with {0}", "params");
				
				writer.ToString().ShouldContain("Debug Message with params");
			}
		}
		
		[TestFixture]
		public class when_logging_info_messages
		{
			StringWriter writer;
			Log log;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				log = new Log(writer, new [] { LogLevel.Info });
			}
			
			[Test]
			public void should_write_message_to_console()
			{
				log.InfoMessage("Info Message");
				
				writer.ToString().ShouldContain("Info Message");
			}
			
			[Test]
			public void should_write_message_with_params_to_console()
			{
				log.InfoMessage("Info Message with {0}", "params");
				
				writer.ToString().ShouldContain("Info Message with params");
			}
		}
		
		public class when_logging_error_messages
		{
			StringWriter writer;
			Log log;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				log = new Log(writer, new [] { LogLevel.Error });
			}
			
			[Test]
			public void should_write_message_to_console()
			{
				log.ErrorMessage("Error Message");
				
				writer.ToString().ShouldContain("Error Message");
			}
			
			[Test]
			public void should_write_message_with_params_to_console()
			{
				log.ErrorMessage("Error Message with {0}", "params");
				
				writer.ToString().ShouldContain("Error Message with params");
			}
			
			[Test]
			public void should_write_exception_message_to_console()
			{
				var exception = new Exception("Test Error Exception");
				
				log.ErrorMessage(exception);
				
				writer.ToString().ShouldContain(exception.Message);
			}
			
			[Test]
			public void should_write_inner_exception_message_to_console()
			{
				var innerExceptionMessage = "Inner Exception Message";
				var exceptionWithInnerException = new Exception("Test Error Exception With Inner Exception", new Exception(innerExceptionMessage));
				
				log.ErrorMessage(exceptionWithInnerException);
				
				writer.ToString().ShouldContain(innerExceptionMessage);
			}
		}
		
		public class when_logging_messages_with_log_level_set_to_off
		{
			StringWriter writer;
			Log log;
			
			[SetUp]
			public void SetUp()
			{
				writer = new StringWriter();
				log = new Log(writer, new [] { LogLevel.Debug, LogLevel.Off });
			}
			
			[Test]
			public void should_not_write_message_to_console()
			{
				log.DebugMessage("Debug Message");
				
				writer.ToString().ShouldNotContain("Debug Message");
			}
		}
	}
}
