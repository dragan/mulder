using System;

using NSubstitute;
using NUnit.Framework;
using Shouldly;

using Mulder.Base.Compilation;
using Mulder.Base.Filters;
using Mulder.Base.IO;

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
				var fileSystem = Substitute.For<IFileSystem>();
				filterFactory = new FilterFactory(fileSystem);
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
			
			[Test]
			public void should_be_able_to_create_less_filter()
			{
				IFilter filter = filterFactory.CreateFilter("less");
				
				filter.ShouldBeTypeOf(typeof(LessFilter));
			}
		}
	}
}

