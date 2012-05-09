using System;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base;
using Mulder.Base.Logging;

namespace Mulder.Tests.Base
{
	public class EntryPointTests
	{
		[TestFixture]
		public class when_calling_run
		{
			ILog log;
			EntryPoint entryPoint;
			
			[SetUp]
			public void SetUp()
			{
				log = Substitute.For<ILog>();
				entryPoint = new EntryPoint(log);
			}
			
			[Test]
			public void should_log_truth_is_out_there_message()
			{
				entryPoint.Run(new string[] {});
				
				log.Received().InfoMessage("The Truth Is Out There");
			}
			
			[Test]
			public void should_return_success_exit_code()
			{
				ExitCode exitCode = entryPoint.Run(new string[] {});
				
				exitCode.ShouldBe(ExitCode.Success);
			}
		}
	}
}
