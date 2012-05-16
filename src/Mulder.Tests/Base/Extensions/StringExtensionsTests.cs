using System;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Extensions;

namespace Mulder.Tests.Base.Extensions
{
	public class StringExtensionsTests
	{
		[TestFixture]
		public class when_chopping_string
		{
			[Test]
			public void should_remove_last_character()
			{
				string expected = "hello";
		
				string actual = "hello/".Chop();
		
				actual.ShouldBe(expected);
			}
		}
	}
}
