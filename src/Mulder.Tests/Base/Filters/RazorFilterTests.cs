using System;
using System.Dynamic;

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

				dynamic model = new ExpandoObject();
				model.Name = "Agent Mulder";

				string result = razorFilter.Execute("Hi @Model.Name!", model);
				
				result.ShouldBe(expected);
			}
		}
	}
}
