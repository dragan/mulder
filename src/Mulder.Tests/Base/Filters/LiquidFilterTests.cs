using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Filters;

namespace Mulder.Tests.Base.Filters
{
	public class LiquidFilterTests
	{
		[TestFixture]
		public class when_executing_liquid_filter
		{
			[Test]
			public void should_be_able_to_transform_source()
			{
				string expected = "Hi Agent Mulder!";
				
				var liquidFilter = new LiquidFilter();
				
				string result = liquidFilter.Execute("Hi {{name}}!", new Dictionary<string, object> { { "name", "Agent Mulder" } });
				
				result.ShouldBe(expected);
			}
		}
	}
}
