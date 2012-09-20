using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Filters;

namespace Mulder.Tests.Base.Filters
{
	public class RazorFilterTests
	{
		[TestFixture]
		public class when_executing_razor_filter
		{
			[Test]
			public void should_be_able_to_transform_source()
			{
				string expected = "Hi Agent Mulder!";
				
				var razorFilter = new RazorFilter();

				string result = razorFilter.Execute("Hi @Model[\"name\"]!", new Dictionary<string, object> { { "name", "Agent Mulder" } });
				
				result.ShouldBe(expected);
			}
		}
	}
}
