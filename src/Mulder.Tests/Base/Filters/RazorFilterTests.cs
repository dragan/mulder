using System;
using System.Collections.Generic;
using System.Dynamic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
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

				dynamic item = new ExpandoObject();
				item.Name = "Agent Mulder";

				string result = razorFilter.Execute("Hi @Model.Item.Name!", new FilterContext {
					Item = item
				});
				
				result.ShouldBe(expected);
			}
		}
	}
}
