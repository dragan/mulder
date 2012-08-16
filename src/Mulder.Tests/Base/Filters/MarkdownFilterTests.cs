using System;
using System.Collections.Generic;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Filters;

namespace Mulder.Tests.Base.Filters
{
	public class MarkdownFilterTests
	{
		[TestFixture]
		public class when_executing_markdown_filter
		{
			[Test]
			public void should_be_able_to_transform_source()
			{
				string expected = "<h3>The Truth Is Out There</h3>";
				
				var markdownFilter = new MarkdownFilter();
				
				string result = markdownFilter.Execute("### The Truth Is Out There", new Dictionary<string, object>());
				
				result.ShouldContain(expected);
			}
		}
	}
}
