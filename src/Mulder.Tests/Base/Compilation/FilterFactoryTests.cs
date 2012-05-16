using System;

using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Filters;

namespace Mulder.Tests.Base.Compilation
{
	public class FilterFactoryTests
	{
		[TestFixture]
		public class when_creating_filters
		{
			IFilterFactory filterFactory;
			
			[SetUp]
			public void SetUp()
			{
				filterFactory = new FilterFactory();
			}
			
			[Test]
			public void should_be_able_to_create_liquid_filter()
			{
				IFilter filter = filterFactory.CreateFilter("liquid");
				
				filter.ShouldBeTypeOf(typeof(LiquidFilter));
			}
			
			[Test]
			public void should_be_able_to_create_markdown_filter()
			{
				IFilter filter = filterFactory.CreateFilter("markdown");
				
				filter.ShouldBeTypeOf(typeof(MarkdownFilter));
			}
		}
	}
}

